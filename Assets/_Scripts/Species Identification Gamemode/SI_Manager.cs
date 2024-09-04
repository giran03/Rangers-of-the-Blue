using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class Species
{
    public string speciesName;
    public string scientificName;
    public string conservationStatus;
    public string kingdom;
}

[Serializable]
public class SpeciesIndex
{
    public Sprite speciesImg;
    public string speciesIndexName;
}

public class SI_Manager : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] TMP_Text label_score;
    [SerializeField] TMP_Text label_currentLevel;
    [SerializeField] GameObject button_index;

    [Header("Species Configs")]
    public Species[] lowConcernSpecies_Array;
    public Species[] endangeredSpecies_Array;
    public Species[] criticallyEndangeredSpecies_Array;
    public SpeciesIndex[] speciesIndex_Array;
    [HideInInspector] public Species[] _species = { };
    [SerializeField] GameObject dispaly_scanInfoBox;
    [SerializeField] GameObject button_scanButton;
    [SerializeField] TMP_Text label_speciesName;
    [SerializeField] TMP_Text label_scientificName;
    [SerializeField] TMP_Text label_conservationStatus;
    [SerializeField] TMP_Text label_kingdom;

    [Header("Quiz Configs")]
    [SerializeField] GameObject[] quizBoxArray;
    GameObject currentQuiz;
    int currentQuizIndex;

    [Header("Object Pooling Configs")]
    [SerializeField] GameObject[] prefab_species; // Prefab to nstantiate for the pool
    [SerializeField] int initialPoolSize = 8; // Target pool size
    [SerializeField] List<GameObject> pool;
    [SerializeField] Vector3 min_SpawnOffset;
    [SerializeField] Vector3 max_SpawnOffset;
    [SerializeField] GameObject origin;
    GameObject spawned_species;

    [Header("Timer Config")]
    [SerializeField] GameObject hud_label_timerHasRunOut;
    [SerializeField] TMP_Text hud_label_timerText;
    public float timeRemaining = 10;
    public bool timerIsRunning;

    [Header("End Screen Configs")]
    [SerializeField] GameObject display_EndScreen;

    public List<Species> ScannedSpeciesCollection = new();
    public Species currentSpecies;

    Vector3 screenCenter = new(0.5f, 0.5f, 0f);

    // scores
    public int score_scanedSpecies;

    //PROFILE CONFIGS
    string profile;
    PlayerData playerData;

    Ray cameraRay;
    bool isScanning;

    public GameObject currentSpecies_Obect;

    public static SI_Manager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        profile = SaveSystem.SelectedProfileName;
        playerData = Profile.Instance.LoadPlayer(profile);

        Debug.Log($"HIGHEST LEVEL OF PLAYER {playerData.playerName} is {playerData.profile_SI_Level} in SI Gamemode!!!");
        Debug.Log($"ENTERED SI GAMEMODE LEVEL {PlayerPrefs.GetInt("SI_SelectedLevel")}");

        _species = lowConcernSpecies_Array.Concat(endangeredSpecies_Array).Concat(criticallyEndangeredSpecies_Array).ToArray();
    }

    private void Start()
    {
        DisplayInfoBox();
        timerIsRunning = true;
        score_scanedSpecies = 0;

        Debug.Log($"Checking scanned species for {playerData.playerName}~~");
        if (playerData.scannedSpeciesList != null)
            foreach (Species species in playerData.scannedSpeciesList)
            {
                Debug.Log($"speciesDataList: {species.speciesName}");

                ScannedSpeciesCollection.Add(species); //add existing species from save file
            }

        foreach (Species item in ScannedSpeciesCollection)
        {
            Debug.Log($"Added {item} to the species collection");
        }

        timeRemaining = SI_Level_Manager.Instance.GetLevelTimer();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GrowPool();
        }

        // 🔊 change music
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Game BGM");
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!\nSaving Player Data~");

                SavePlayerData();
            }
        }

        if (!timerIsRunning) return;
        cameraRay = Camera.main.ViewportPointToRay(screenCenter);
        if (Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Species"))
            {
                Debug.Log($"Currently scanning species~");
            }
        }
        else
        {
            DisplayInfoBox();
            button_scanButton.SetActive(true);
            button_index.SetActive(true);
            currentSpecies_Obect = null;
            DestroyQuiz();

            if (isScanning)
            {
                isScanning = false;
                GameSceneManager.Instance.ResumeGame();
            }
        }
        // sets score and level
        if (score_scanedSpecies >= 0)
            label_score.SetText($"SCORE: {score_scanedSpecies}");
        else score_scanedSpecies = 0;

        label_currentLevel.SetText($"LEVEL {PlayerPrefs.GetInt("SI_SelectedLevel") + 1}");
    }

    // BUTTON
    /// <summary>
    /// Scans the species
    /// </summary>
    public void Button_ScanButton()
    {
        if (Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Species"))
            {
                currentSpecies_Obect = hit.collider.gameObject;

                if (currentSpecies_Obect.TryGetComponent<SpeciesName>(out SpeciesName speciesObj))
                {
                    CheckScanned(speciesObj);

                    if (speciesObj.scanned) // if species is already scanned -> initiate QUIZ
                    {
                        Debug.Log($"This species is already scanned!\n~Initiating Quiz!");
                        button_scanButton.SetActive(false);

                        GetSpeciesInfo(speciesObj.speciesName);

                        //RANDOM QUIZ
                        int temp1 = UnityEngine.Random.Range(0, quizBoxArray.Length);
                        int temp2 = UnityEngine.Random.Range(0, quizBoxArray.Length / 2);
                        int randomQuizIndex = UnityEngine.Random.Range(temp1, temp2);

                        Canvas canvas = FindObjectOfType<Canvas>();
                        Vector3 quizPos = new(0, 320, canvas.transform.position.z);
                        currentQuiz = Instantiate(quizBoxArray[randomQuizIndex], quizPos, canvas.transform.rotation);
                        currentQuiz.transform.SetParent(canvas.transform, false);

                        currentQuizIndex = randomQuizIndex;

                        Debug.Log($"Current quiz is: {currentQuiz.name}");

                        // disable index button
                        button_index.SetActive(false);

                        // 🔊 SFX
                        AudioManager.Instance.PlaySFX("Information Ping");
                    }
                    else
                    {
                        DisplayInfoBox(true);
                        button_scanButton.SetActive(false);
                        DestroyQuiz();

                        GetSpeciesInfo(speciesObj.speciesName);
                        StartCoroutine(SetScanned(speciesObj));

                        button_index.SetActive(true);
                    }
                    isScanning = true;
                    GameSceneManager.Instance.PauseGame();
                    Debug.Log($"GAME PAUSED!!!!!");
                }
            }
            else
                currentSpecies_Obect = null;
        }
    }

/// <summary>
/// Display Information about the species
/// </summary>
    void DisplayInfoBox(bool condition = false)
    {
        dispaly_scanInfoBox.SetActive(condition);
    }

    public void Button_CloseInformation_PopUp()
    {
        DisplayInfoBox(false);
        isScanning = false;
        GameSceneManager.Instance.ResumeGame();
    }

    /// <summary>
    /// Destroys the current Quiz
    /// </summary>
    public void DestroyQuiz() => Destroy(currentQuiz);

    IEnumerator SetScanned(SpeciesName speciesName)
    {
        yield return new WaitForSeconds(2);
        speciesName.scanned = true;
        // IndexHandler.Instance.RefreshIndex();
    }

    void CheckScanned(SpeciesName speciesName)
    {
        foreach (Species species in ScannedSpeciesCollection)
        {
            Debug.Log($"CHECKING SCANNED SPECIES: {species.speciesName} comparator: {speciesName.speciesName}");
            if (species.speciesName.Contains(speciesName.speciesName))
                speciesName.scanned = true;
        }
    }

    // TIMER
    /// <summary>
    /// Displays and sets the timer text
    /// </summary>
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        hud_label_timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    /// <summary>
    /// Gets the species detailed information
    /// </summary>
    void GetSpeciesInfo(string speciesName)
    {
        currentSpecies = Array.Find(_species, x => x.speciesName == speciesName);
        if (currentSpecies != null)
        {
            DisplaySpeciesInfo();

            if (!ScannedSpeciesCollection.Any(x => x.speciesName == currentSpecies.speciesName))
            {
                ScannedSpeciesCollection.Add(currentSpecies);
                // play species SFX
                AudioManager.Instance.PlaySFX(currentSpecies.speciesName);
            }
            else
                Debug.Log($"Species is already in the collection!");
        }
        else
            Debug.Log("Cant find species data");
    }

    void DisplaySpeciesInfo()
    {
        label_speciesName.SetText($"{currentSpecies.speciesName}");
        label_scientificName.SetText($"{currentSpecies.scientificName}");
        label_conservationStatus.SetText($"{currentSpecies.conservationStatus}");
        label_kingdom.SetText($"{currentSpecies.kingdom}");
    }

    /// <summary>
    /// Gamemode Scoring
    /// </summary>
    public void SpeciesScoring(string speciesName, bool isCorrect)
    {
        /*
            SCORING GUIDE
            Critically Endangered   : 30 pts
            Endangered              : 15 pts
            Least Concern Species   : 5 pts
        */
        int criticallyEndangered = 30;
        int endangered = 15;
        int leastConcern = 5;

        // destroy species obj
        Debug.Log($"Destroying Species~");
        Destroy(currentSpecies_Obect);
        OnObjectDestroyed(currentSpecies_Obect);

        switch (speciesName)
        {
            // least concern species
            case "Milkfish":
                Debug.Log($"Scanned Milkfish~ {leastConcern} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += leastConcern : score_scanedSpecies -= leastConcern;
                break;
            case "Tilapia":
                Debug.Log($"Scanned Tilapia~ {leastConcern} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += leastConcern : score_scanedSpecies -= leastConcern;
                break;
            case "Red Snapper":
                Debug.Log($"Scanned Red Snapper~ {leastConcern} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += leastConcern : score_scanedSpecies -= leastConcern;
                break;
            case "Brown Shrimp":
                Debug.Log($"Scanned Brown Shrimp~ {leastConcern} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += leastConcern : score_scanedSpecies -= leastConcern;
                break;
            case "Mud Crab":
                Debug.Log($"Scanned Mud Crab~ {leastConcern} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += leastConcern : score_scanedSpecies -= leastConcern;
                break;
            case "Black Longspine Sea Urchin":
                Debug.Log($"Scanned lack Longspine Sea Urchin~ {leastConcern} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += leastConcern : score_scanedSpecies -= leastConcern;
                break;

            // endangered species
            case "Green Sea Turtle":
                Debug.Log($"Scanned Green Sea Turtle~ {endangered} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += endangered : score_scanedSpecies -= 10;
                break;
            case "Bluefin Tuna":
                Debug.Log($"Scanned Bluefin Tuna~ {endangered} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += endangered : score_scanedSpecies -= 10;
                break;
            case "Whale Shark":
                Debug.Log($"Scanned Whale Shark~ {endangered} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += endangered : score_scanedSpecies -= 10;
                break;
            // critically endangered species
            case "Sunflower Sea Star":
                Debug.Log($"Scanned Sunflower Sea Star~ {criticallyEndangered} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += criticallyEndangered : score_scanedSpecies -= 15;
                break;
            case "Vaquita":
                Debug.Log($"Scanned Vaquita~ {criticallyEndangered} points");
                score_scanedSpecies = isCorrect ? score_scanedSpecies += criticallyEndangered : score_scanedSpecies -= 15;
                break;
        }

        Debug.Log($"SCORE: {score_scanedSpecies}");

        Debug.Log($"The Local Scanned Species Collection Count is {ScannedSpeciesCollection.Count}");
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(1.2f);
        display_EndScreen.SetActive(true);

        // 🔊 SFX
        AudioManager.Instance.PlaySFX("Score Up");
    }

    /// <summary>
    /// Updates and saves the player progress in savefile
    /// </summary>
    void SavePlayerData()
    {
        PlayerData newData = new();

        switch (PlayerPrefs.GetInt("SI_SelectedLevel"))
        {
            case 0:
                if (playerData.profile_SI_Level < 1)
                {
                    newData.profile_SI_Level = 0;
                    Debug.Log($"Change level save to {newData.profile_SI_Level}");
                }
                else newData.profile_SI_Level = 2;

                newData.scannedSpeciesList = ScannedSpeciesCollection;

                // LEVEL CLEAR CHECKS
                if (playerData.stage_SI_2_cleared)
                    newData.stage_SI_2_cleared = true;

                if (playerData.stage_SI_3_cleared)
                    newData.stage_SI_3_cleared = true;

                newData.stage_SI_1_cleared = true;

                newData.profile_SI_Level_1_Score = score_scanedSpecies;
                Debug.Log($"Im saving for {playerData.playerName} with {newData.profile_SI_Level_1_Score}");
                break;

            case 1:
                if (playerData.profile_SI_Level < 1)
                {
                    newData.profile_SI_Level = 0;
                    Debug.Log($"Change level save to {newData.profile_SI_Level}");
                }
                else newData.profile_SI_Level = 2;

                newData.scannedSpeciesList = ScannedSpeciesCollection;

                // LEVEL CLEAR CHECKS
                if (playerData.stage_SI_3_cleared)
                    newData.stage_SI_3_cleared = true;

                newData.stage_SI_1_cleared = true;
                newData.stage_SI_2_cleared = true;

                newData.profile_SI_Level_2_Score = score_scanedSpecies;
                Debug.Log($"Im saving for {playerData.playerName} with {newData.profile_SI_Level_2_Score}");
                break;

            case 2:
                if (playerData.profile_SI_Level < 1)
                {
                    newData.profile_SI_Level = 0;
                    Debug.Log($"Change level save to {newData.profile_SI_Level}");
                }
                else newData.profile_SI_Level = 2;

                newData.scannedSpeciesList = ScannedSpeciesCollection;

                // LEVEL CLEAR CHECKS

                newData.stage_SI_1_cleared = true;
                newData.stage_SI_2_cleared = true;
                newData.stage_SI_3_cleared = true;

                newData.profile_SI_Level_3_Score = score_scanedSpecies;
                Debug.Log($"Im saving for {playerData.playerName} with {newData.profile_SI_Level_3_Score}");
                break;
        }

        // updates the current selected player data
        Profile.Instance.UpdateData_SI(newData);

        timerIsRunning = false;

        StartCoroutine(GameEnd());
    }

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
            int randomIndex = UnityEngine.Random.Range(0, prefab_species.Length);
            prefabToSpawn = prefab_species[randomIndex];

            // spawning trash
            spawned_species = Instantiate(prefabToSpawn, transform);
            spawned_species.SetActive(true);
            spawned_species.transform.SetParent(origin.transform);
            pool.Add(spawned_species);

            // random position
            x = UnityEngine.Random.Range(min_SpawnOffset.x, max_SpawnOffset.x);
            y = UnityEngine.Random.Range(-.3f, .3f);
            z = UnityEngine.Random.Range(min_SpawnOffset.z, max_SpawnOffset.z);
            pos = new Vector3(x, y, z);
            spawned_species.transform.position = pos;

            Debug.Log("Spawned another species~!");
        }
    }

    public void OnObjectDestroyed(GameObject obj)
    {
        pool.Remove(obj); // Remove destroyed object from the pool
        GrowPool(); // Ensure pool size is maintained
    }
}