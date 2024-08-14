using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SI_EndScreenHandler : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] GameObject button_next;
    [SerializeField] TMP_Text label_currentScore;
    [SerializeField] TMP_Text label_levelHighscore;
    [SerializeField] TMP_Text label_currentLevel;

    PlayerData playerData;

    private void Start()
    {
        playerData = Profile.Instance.LoadPlayer(SaveSystem.SelectedProfileName);
    }

    private void Update()
    {
        label_currentLevel.SetText($"Level {PlayerPrefs.GetInt("SI_SelectedLevel") + 1}");
        label_currentScore.SetText($"Score: {SI_Manager.Instance.score_scanedSpecies}");
        if (PlayerPrefs.GetInt("SI_SelectedLevel") == 0)
            label_levelHighscore.SetText($"Highscore: {playerData.profile_SI_Level_1_Score}");
        if (PlayerPrefs.GetInt("SI_SelectedLevel") == 1)
            label_levelHighscore.SetText($"Highscore: {playerData.profile_SI_Level_2_Score}");
        if (PlayerPrefs.GetInt("SI_SelectedLevel") == 2)
            label_levelHighscore.SetText($"Highscore: {playerData.profile_SI_Level_3_Score}");

        if (PlayerPrefs.GetInt("SI_SelectedLevel") == 2)
            button_next.SetActive(false);
        else
            button_next.SetActive(true);
    }

    public void Button_MainMenu()
    {
        SaveSystem.ResetSelectedProfile();
        GameSceneManager.Instance.ReturnMainMenu();
    }
    public void Button_NextLevel()
    {
        int temp = PlayerPrefs.GetInt("SI_SelectedLevel");
        if (temp < 3)
            PlayerPrefs.SetInt("SI_SelectedLevel", temp + 1);

        Debug.Log($"SI Next Level value: {PlayerPrefs.GetInt("SI_SelectedLevel")}");

        SI_Level_Manager.Instance.NextLevel(temp);

        Button_ReplayLevel();
    }
    public void Button_ReplayLevel()
    {
        GameSceneManager.Instance.RestartLevel();
    }
}
