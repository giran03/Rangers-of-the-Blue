using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp_Handler : MonoBehaviour
{
    [SerializeField] Camera arCamera;

    [Header("Configs")]
    [SerializeField] GameObject hud_icon_grab;
    [SerializeField] GameObject hud_icon_deposit;
    [SerializeField] GameObject hud_button_grab;
    [SerializeField] TMP_Text hud_label_currentLevel;
    [SerializeField] TMP_Text hud_label_trashPickUp;
    [SerializeField] TMP_Text hud_label_netBag;
    [SerializeField] GameObject hud_label_netBagFull;

    [Header("Timer Config")]
    [SerializeField] GameObject hud_label_timerHasRunOut;
    [SerializeField] TMP_Text hud_label_timerText;
    public float timeRemaining = 10;
    public bool timerIsRunning;

    [Header("Deposit Button Configs")]
    [SerializeField] GameObject button_DisposeButton;

    // object pooling
    [Header("Object Pooling Configs")]
    [SerializeField] GameObject[] prefab_trash; // Prefab to instantiate for the pool
    [SerializeField] int initialPoolSize = 8; // Target pool size
    [SerializeField] List<GameObject> pool;
    [SerializeField] Vector3 min_SpawnOffset;
    [SerializeField] Vector3 max_SpawnOffset;
    [SerializeField] Transform poolSpawnOrigin;
    GameObject spawned_trash;
    GameObject trashObject;

    [Header("Net Configs")]
    [SerializeField] int netBag_capacity = 10;
    int netBag_currentCapacity;
    bool isBagFullText;

    [Header("End Screen Configs")]
    [SerializeField] GameObject display_EndScreen;

    // raycasts
    RaycastHit hit;
    string lastCollected_Trash;

    // scores
    public static int score_trashPickUp;
    int currentScore;

    string profile;
    PlayerData data;

    Vector3 screenCenter = new(0.5f, 0.5f, 0f);

    public static PickUp_Handler Instance;

    #region GENERAL
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        Debug.Log($"Selected Profile is: {SaveSystem.SelectedProfileName}");

        profile = SaveSystem.SelectedProfileName;
        data = Profile.Instance.LoadPlayer(profile);
        // data = SaveSystem.LoadPlayer("Inannis");
        Debug.Log($"HIGHEST LEVEL OF PLAYER {data.playerName} is {data.profile_TP_Level} !!!");
    }

    private void Start()
    {
        timerIsRunning = true;
        score_trashPickUp = 0;
        currentScore = 0;
        netBag_currentCapacity = 0;

        display_EndScreen.SetActive(false);

        // 🔊 change music
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Game BGM");
    }

    void Update()
    {
        //TIMER
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                hud_label_timerHasRunOut.SetActive(true);
                timeRemaining = 0;

                PlayerData newData = new();

                switch (PlayerPrefs.GetInt("TP_SelectedLevel"))
                {
                    case 0:
                        if (data.profile_TP_Level < 1)
                        {
                            newData.profile_TP_Level = 0;
                            Debug.Log($"Change level save to {newData.profile_TP_Level}");
                        }
                        else newData.profile_TP_Level = 2;

                        // LEVEL CLEAR CHECKS
                        if (data.stage_2_cleared)
                            newData.stage_2_cleared = true;

                        if (data.stage_3_cleared)
                            newData.stage_3_cleared = true;

                        newData.stage_1_cleared = true;

                        newData.profile_TP_Level_1_Score = score_trashPickUp;
                        Debug.Log($"Im saving for {data.playerName} with {newData.profile_TP_Level_1_Score}");
                        break;

                    case 1:
                        if (data.profile_TP_Level < 2)
                        {
                            newData.profile_TP_Level = 1;
                            Debug.Log($"Change level save to {newData.profile_TP_Level}");
                        }
                        else newData.profile_TP_Level = 2;

                        // LEVEL CLEAR CHECKS
                        if (data.stage_3_cleared)
                            newData.stage_3_cleared = true;

                        newData.stage_1_cleared = true;
                        newData.stage_2_cleared = true;

                        newData.profile_TP_Level_2_Score = score_trashPickUp;
                        Debug.Log($"Im saving for {data.playerName} with {newData.profile_TP_Level_2_Score}");
                        break;

                    case 2:
                        newData.profile_TP_Level = 2; // max
                        Debug.Log($"Change level save to {newData.profile_TP_Level}");

                        newData.stage_1_cleared = true;
                        newData.stage_2_cleared = true;
                        newData.stage_3_cleared = true;

                        newData.profile_TP_Level_3_Score = score_trashPickUp;
                        Debug.Log($"Im saving for {data.playerName} with {newData.profile_TP_Level_3_Score}");
                        break;
                }

                Debug.Log($"Saving data for {data.playerName} with score of {score_trashPickUp} for level {data.profile_TP_Level + 1}");

                // updates the current selected player data
                Profile.Instance.UpdateData(newData);

                timerIsRunning = false;

                StartCoroutine(GameEnd());
            }
        }

        if (!timerIsRunning) return;
        Ray ray = arCamera.ViewportPointToRay(screenCenter);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            /*
                [TRASH SCORING]
                Trash_Low       = 5 pts
                Trash_Mid       = 10 pts
                Trash_High      = 20 pts

                Shell           = -5 pts
                Urchin          = -10 pts
            */
            CheckCollectedTrash("Trash_Low");
            CheckCollectedTrash("Trash_Mid");
            CheckCollectedTrash("Trash_High");

            CheckCollectedTrash("Trash_Avoid_Shell");
            CheckCollectedTrash("Trash_Avoid_Urchin");

            // checks if the collided object has the word "Trash"
            if (hit.collider.tag.Contains("Trash"))
            {
                hud_icon_grab.SetActive(true);
                hud_button_grab.SetActive(true);
            }
            if (hit.collider.CompareTag("Deposit"))
            {
                button_DisposeButton.SetActive(true);
                hud_icon_deposit.SetActive(true);
                Debug.Log("Deposit trash Here~!");
            }
        }
        else
        {
            button_DisposeButton.SetActive(false);
            hud_icon_deposit.SetActive(false);
            hud_icon_grab.SetActive(false);
            hud_button_grab.SetActive(false);
        }

        // sets score and level
        hud_label_currentLevel.SetText($"LEVEL {PlayerPrefs.GetInt("TP_SelectedLevel") + 1}");
        hud_label_trashPickUp.SetText($"SCORE: {score_trashPickUp}");
        hud_label_netBag.SetText($"{netBag_currentCapacity}");
    }
    #endregion

    public void PickUpButton()
    {
        if (trashObject != null)
        {
            if (netBag_currentCapacity != netBag_capacity)
            {
                netBag_currentCapacity++;

                // trash scoring
                CollectedTrashScoring("Trash_Low", 5);
                CollectedTrashScoring("Trash_Mid", 10);
                CollectedTrashScoring("Trash_High", 20);

                CollectedTrashScoring("Trash_Avoid_Shell", -5);
                CollectedTrashScoring("Trash_Avoid_Urchin", -10);

                Destroy(trashObject);
                OnObjectDestroyed(trashObject); // respawns object

                // 🔊 SFX
                AudioManager.Instance.PlaySFX("Trash PickUp");

                // 🔊 SFX
                if (lastCollected_Trash.Contains("Avoid"))
                    AudioManager.Instance.PlaySFX("Wrong");
                else
                    AudioManager.Instance.PlaySFX("Correct");
            }
            else
            {
                Debug.Log("The net bag is full!!!\nDeposit trash first");
                if (!isBagFullText)
                    StartCoroutine(NetBagFull());

                // 🔊 SFX
                AudioManager.Instance.PlaySFX("Notification Full");
            }
        }
    }

    void CheckCollectedTrash(string trashType)
    {
        if (hit.collider.CompareTag(trashType))
        {
            // set the last collected trash from the argument
            lastCollected_Trash = trashType;
            trashObject = hit.collider.gameObject;
        }
    }

    void CollectedTrashScoring(string trashType, int score)
    {
        if (lastCollected_Trash == trashType)
        {
            Debug.Log($"Picked up {trashType} trash~ + {score} pts");
            currentScore += score;
        }
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(1.2f);
        display_EndScreen.SetActive(true);

        // 🔊 SFX
        AudioManager.Instance.PlaySFX("Score Up");
    }

    public void DepositButton()
    {
        // set score
        // score_trashPickUp += netBag_currentCapacity;
        if (lastCollected_Trash != null)
            score_trashPickUp += currentScore;
        else
            Debug.Log($"Current score is low!! {currentScore}");

        // reset conditions and score
        lastCollected_Trash = null;
        currentScore = 0;

        netBag_currentCapacity = 0;

        // 🔊 SFX
        AudioManager.Instance.PlaySFX("Trash Dump");
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        hud_label_timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    IEnumerator NetBagFull()
    {
        isBagFullText = true;
        hud_label_netBagFull.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        isBagFullText = false;
        hud_label_netBagFull.SetActive(false);
    }

    #region Object Pooling
    private void GrowPool()
    {
        if (pool.Count < initialPoolSize)
        {
            float x;
            float y;
            float z;
            Vector3 pos;

            GameObject prefabToSpawn;

            // add prefabs spawn to other trash
            int randomIndex = Random.Range(0, prefab_trash.Length);
            prefabToSpawn = prefab_trash[randomIndex];

            // spawning trash
            spawned_trash = Instantiate(prefabToSpawn, transform);
            spawned_trash.SetActive(true);
            pool.Add(spawned_trash);

            // random position
            x = Random.Range(min_SpawnOffset.x, max_SpawnOffset.x);
            y = Random.Range(-.3f, .3f);
            z = Random.Range(min_SpawnOffset.z, max_SpawnOffset.z);
            pos = new Vector3(x, y, z);
            spawned_trash.transform.position = pos;

            Debug.Log("Spawned another trash~!");
        }
    }

    public void OnObjectDestroyed(GameObject obj)
    {
        pool.Remove(obj); // Remove destroyed object from the pool
        GrowPool(); // Ensure pool size is maintained
    }
    #endregion
}
