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
                Debug.Log($"Button of {_buttonName} is pressed!");
                NextButton(TP_levelButtonsList);
                break;
            case "SI Button":
                Debug.Log($"Button of {_buttonName} is pressed!");
                NextButton(SI_levelButtonsList);
                break;
        }
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
        }

        Debug.Log($"PRESSED BUTTON {_buttonName} of level {PlayerPrefs.GetInt("TP_SelectedLevel")}");
    }

    public void Button_ResetSelectedProfile()
    {
        if (SaveSystem.SelectedProfileName != null)
            SaveSystem.SelectedProfileName = null;
        LeaderboardHandler.Instance.RefreshLeaderboards();
    }

    void NextButton(List<GameObject> gameObjectsList)
    {
        string profile = SaveSystem.SelectedProfileName;
        PlayerData data = SaveSystem.LoadPlayer(profile);

        Debug.Log($"Highest level of {data.playerName} in TP GAMEMODE is: {data.profile_TP_Level}");
        
        if (data.stage_2_cleared)
        {
            Debug.Log($"Enabling 3 Buttons");
            DeActivateButton(gameObjectsList);
            ActivateButton(gameObjectsList, 3);
        }
        else if (data.stage_1_cleared)
        {
            Debug.Log($"Enabling 2 Buttons");
            DeActivateButton(gameObjectsList);
            ActivateButton(gameObjectsList, 2);
        }
        else
        {
            Debug.Log($"Enabling 1 Buttons");
            DeActivateButton(gameObjectsList);
            ActivateButton(gameObjectsList, 1);
        }
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
