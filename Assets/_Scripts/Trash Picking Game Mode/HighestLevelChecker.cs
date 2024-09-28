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

    private void Start()
    {
        PlayerPrefs.SetInt("TP_SelectedLevel", 0);
        PlayerPrefs.SetInt("SI_SelectedLevel", 0);
    }


    public void ButtonPress(Button button)
    {
        TP_LevelSelectHandler.isDataLoaded = false;
        if (SaveSystem.SelectedProfileName == null)
        {
            return;
        }

        switch (_buttonName = button.name)
        {
            case "TP Play Folder":
                NextButton("TP");
                break;

            case "SI Play Folder":
                NextButton("SI");
                break;
        }
    }

    // BUTTON | setting the selected level before playing;
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

        if (LeaderboardHandler.Instance != null)
            LeaderboardHandler.Instance.RefreshLeaderboards();
    }

    void NextButton(string selectedGamemode)
    {
        PlayerData data;
        data = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);
        switch (selectedGamemode)
        {
            case "TP":
                CheckLevelProgress(data, selectedGamemode);
                break;

            case "SI":
                CheckLevelProgress(data, selectedGamemode);
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
        for (int i = 0; i < gameObjectList.Count; i++)
            gameObjectList[i].SetActive(false);
    }
}
