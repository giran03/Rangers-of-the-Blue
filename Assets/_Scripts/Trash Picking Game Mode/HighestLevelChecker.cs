using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighestLevelChecker : MonoBehaviour
{
    // Trash Picking Gamemode
    [SerializeField] List<GameObject> TP_levelButtonsList;

    // Species Identification Gamemode
    [SerializeField] List<GameObject> SI_levelButtonsList;

    string _buttonName;

    public void ButtonPress(Button button)
    {
        TP_LevelSelectHandler.isDataLoaded = false;
        if (SaveSystem.SelectedProfileName == null)
        {
            Debug.Log("Selected profile is NULL!");
            return;
        }

        switch (_buttonName = button.name)
        {
            case "TP Button":
                NextButton("TP");
                break;
                
            case "SI Button":
                NextButton("SI");
                break;
        }

        Debug.Log($"Button of {_buttonName} is pressed!");
    }

    // BUTTON | setting the selected level before playing; saved in string key "TP_SelectedLevel" in PlayerPrefs.
    public void Button_LevelButtonPress(string buttonLevel)
    {
        switch (buttonLevel)
        {
            case "TP 1":
                PlayerPrefs.SetInt("TP_SelectedLevel", 0);
                break;
            case "TP 2":
                PlayerPrefs.SetInt("TP_SelectedLevel", 1);
                break;
            case "TP 3":
                PlayerPrefs.SetInt("TP_SelectedLevel", 2);
                break;

            case "SI 1":
                PlayerPrefs.SetInt("SI_SelectedLevel", 0);
                break;
            case "SI 2":
                PlayerPrefs.SetInt("SI_SelectedLevel", 1);
                break;
            case "SI 3":
                PlayerPrefs.SetInt("SI_SelectedLevel", 2);
                break;
        }
    }

    public void Button_ResetSelectedProfile()
    {
        if (SaveSystem.SelectedProfileName != null)
            SaveSystem.SelectedProfileName = null;
        LeaderboardHandler.Instance.RefreshLeaderboards();
    }

    void NextButton(string selectedGamemode)
    {
        string profile = SaveSystem.SelectedProfileName;
        PlayerData data = SaveSystem.LoadPlayer(profile);

        switch (selectedGamemode)
        {
            case "TP":
                CheckLevelProgress(data, selectedGamemode);
                Debug.Log($"Highest level of {data.playerName} in TP GAMEMODE is: {data.profile_TP_Level}");
                break;

            case "SI":
                CheckLevelProgress(data, selectedGamemode);
                Debug.Log($"Highest level of {data.playerName} in SI GAMEMODE is: {data.profile_SI_Level}");
                break;
        }
    }

    void CheckLevelProgress(PlayerData data, string selectedGamemode)
    {
        switch (selectedGamemode)
        {
            case "TP":
                if (data.stage_2_cleared)
                    FlipButtons(TP_levelButtonsList, 3);
                else if (data.stage_1_cleared)
                    FlipButtons(TP_levelButtonsList, 2);
                else
                    FlipButtons(TP_levelButtonsList, 1);
                break;

            case "SI":
                if (data.stage_SI_2_cleared)
                    FlipButtons(SI_levelButtonsList, 3);
                else if (data.stage_SI_1_cleared)
                    FlipButtons(SI_levelButtonsList, 2);
                else
                    FlipButtons(SI_levelButtonsList, 1);
                break;
        }
    }

    void FlipButtons(List<GameObject> gameObjectsList, int count)
    {
        DeActivateButton(gameObjectsList);
        ActivateButton(gameObjectsList, count);
    }

    void ActivateButton(List<GameObject> gameObjectList, int level)
    {
        for (int i = 0; i < level; i++)
            gameObjectList[i].SetActive(true);
    }


    void DeActivateButton(List<GameObject> gameObjectList)
    {
        Debug.Log("Deactivating buttons~~~~~");
        for (int i = 0; i < gameObjectList.Count; i++)
            gameObjectList[i].SetActive(false);
    }
}
