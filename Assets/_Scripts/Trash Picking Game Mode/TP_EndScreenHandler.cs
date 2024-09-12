using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TP_EndScreenHandler : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] GameObject button_next;
    [SerializeField] GameObject button_finishLevel;
    [SerializeField] TMP_Text label_currentScore;
    [SerializeField] TMP_Text label_levelHighscore;
    [SerializeField] TMP_Text label_currentLevel;

    [Header("End Screen Tips")]
    [SerializeField] TMP_Text textBox_randomTips;
    [SerializeField] string[] randomTips;
    int randomIndex;

    PlayerData playerData;

    private void Start()
    {
        playerData = Profile.Instance.LoadPlayer(SaveSystem.SelectedProfileName);
        randomIndex = Random.Range(0, randomTips.Length);
        button_finishLevel.SetActive(false);
    }

    private void Update()
    {
        label_currentLevel.SetText($"Level {PlayerPrefs.GetInt("TP_SelectedLevel") + 1}");

        label_currentScore.SetText($"Score: {PickUp_Handler.score_trashPickUp}");

        if (PlayerPrefs.GetInt("TP_SelectedLevel") == 0)
            label_levelHighscore.SetText($"Highscore: {playerData.profile_TP_Level_1_Score}");
        if (PlayerPrefs.GetInt("TP_SelectedLevel") == 1)
            label_levelHighscore.SetText($"Highscore: {playerData.profile_TP_Level_2_Score}");
        if (PlayerPrefs.GetInt("TP_SelectedLevel") == 2)
            label_levelHighscore.SetText($"Highscore: {playerData.profile_TP_Level_3_Score}");

        if (PlayerPrefs.GetInt("TP_SelectedLevel") == 2)
        {
            button_next.SetActive(false);
            button_finishLevel.SetActive(true);
        }
        else
            button_next.SetActive(true);

        textBox_randomTips.SetText($"{randomTips[randomIndex]}");
    }

    public void Button_MainMenu()
    {
        SaveSystem.ResetSelectedProfile();
        GameSceneManager.Instance.ReturnMainMenu();
    }
    public void Button_NextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("TP_SelectedLevel");
        if (currentLevel < 3)
            PlayerPrefs.SetInt("TP_SelectedLevel", currentLevel + 1);
        Debug.Log($"Temp value: {PlayerPrefs.GetInt("TP_SelectedLevel")}");

        TrashpickingGameplayManager.Instance.NextLevel(currentLevel);
        Button_ReplayLevel();
    }
    public void Button_ReplayLevel()
    {
        GameSceneManager.Instance.RestartLevel();
    }

    public void Button_FinishLevel()
    {
        PlayerPrefs.SetString("currentPlayer", playerData.playerName);
        GameSceneManager.Instance.FinishLevel();
    }
}
