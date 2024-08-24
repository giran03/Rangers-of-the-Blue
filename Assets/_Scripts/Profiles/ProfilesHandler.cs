using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilesHandler : MonoBehaviour
{
    public static ProfilesHandler Instance;
    [SerializeField] InputField inputField_name;
    [SerializeField] InputField inputField_age;
    [SerializeField] Dropdown dropdown_Profiles;
    string _name;
    int _age;
    public string selectedProfile;
    bool isCreatingProfile = true;

    [Header("Button Configs")]
    [SerializeField] List<GameObject> nextButtonTransitionDisplay;
    [SerializeField] List<GameObject> createButtonTransitionDisplay;

    [SerializeField] InputField inputField_profileCheck;

    [Header("Configs")]
    [SerializeField] TMP_Text label_profileName;
    [SerializeField] GameObject lockedButton;
    [SerializeField] GameObject dialogue_SI_Unlocked;

    private void Awake()
    {
        // reset the selected profile player prefs | TODO: remove this; call it when returning to mainmenu or back button
        SaveSystem.SelectedProfileName = null;
    }

    private void Start()
    {
        UpdateOptions();
        Debug.Log($"Selected profile from memory is: {SaveSystem.SelectedProfileName}");
        
        dialogue_SI_Unlocked.SetActive(false);
    }

    private void Update()
    {
        if (!isCreatingProfile)
            GetDropdownValue();

        try
        {
            _name = inputField_name.text;
            _age = System.Convert.ToInt32(inputField_age.text);
        }
        catch (System.Exception)
        {
            Debug.Log("Please fill up the forms correctly~");
            inputField_age.text = "";
            return;
        }
    }

    public void UpdateOptions()
    {
        List<string> fileNames = GetPlayerFileNamesWithoutExtension();

        if (dropdown_Profiles.options.Count > 0)
            dropdown_Profiles.ClearOptions();

        // Add each filename as a new dropdown option
        foreach (string fileName in fileNames)
        {
            Dropdown.OptionData option = new(fileName);
            dropdown_Profiles.options.Add(new Dropdown.OptionData(option.text, image: null));
        }
        dropdown_Profiles.RefreshShownValue();
    }

    public void Button_CreateNewProfile()
    {
        isCreatingProfile = true;
        selectedProfile = null;
    }
    public void Button_ExistingProfile() => isCreatingProfile = false;

    public void GetDropdownValue()
    {
        Debug.Log($"Dropdown val: {dropdown_Profiles.options[dropdown_Profiles.value].text}");
        if (dropdown_Profiles.options.Count != 0)
        {
            selectedProfile = dropdown_Profiles.options[dropdown_Profiles.value].text;
            Debug.Log($"Dropdown Selected: {selectedProfile}");
        }
    }

    // new profile create button
    public void Button_CreateProfile()
    {
        if (_name == null) return;

        if (_name == "deleteAll")
        {
            SaveSystem.DeleteAllFiles(_name);
            LeaderboardHandler.Instance.RefreshLeaderboards();
            inputField_name.text = "";
            inputField_age.text = "";
            Debug.Log($"Deleted all files!");
            return;
        }
        else
        {
            Profile.Instance.playerName = _name;
            Profile.Instance.playerAge = _age;
            Profile.Instance.SavePlayer();
            selectedProfile = _name;

            inputField_name.text = "";
            inputField_age.text = "";

            Button_SelectThisProfile();
            UpdateOptions();
            Debug.Log($"NEW PROFILE CREATED! name: {Profile.Instance.playerName} , age: {Profile.Instance.playerAge}");

            DisplayTransition(createButtonTransitionDisplay);
        }
    }

    public void Button_NextButton()
    {
        if (selectedProfile == null)
        {
            Debug.Log($"No selected profile!!!");
            return;
        }
        DisplayTransition(nextButtonTransitionDisplay);
    }

    public void Button_SelectThisProfile()
    {
        SaveSystem.SelectedProfileName = selectedProfile;
        PlayerData playerData = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);
        label_profileName.SetText($"GOODLUCK {playerData.playerName}!");
        if (playerData.stage_3_cleared)
        {
            lockedButton.SetActive(false);
            dialogue_SI_Unlocked.SetActive(true);
        }

        ReloadObjects.ProfileToCheck = playerData;
    }

    public static List<string> GetPlayerFileNamesWithoutExtension()
    {
        List<string> fileNames = new();
        string[] potentialFiles = Directory.EnumerateFiles(Application.persistentDataPath, "*.fish").ToArray();

        if (potentialFiles.Length == 0)
        {
            Debug.Log("No .fish files found in persistent data path.");
            return fileNames;
        }

        foreach (string filePath in potentialFiles)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            fileNames.Add(fileNameWithoutExtension);
        }

        return fileNames;
    }

    public void DisplayTransition(List<GameObject> displayList)
    {
        Debug.Log($"Selected profile is {selectedProfile}... PROCEEDING~");
        foreach (var item in displayList)
            item.SetActive(!item.activeSelf);
    }

    #region EDITOR ONLY
    public static void PrintPlayerFileNames()
    {
        string[] potentialFiles = Directory.EnumerateFiles(Application.persistentDataPath, "*.fish").ToArray();

        if (potentialFiles.Length == 0)
        {
            Debug.Log("No .fish files found in persistent data path.");
            return;
        }

        foreach (string filePath in potentialFiles)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            Debug.Log($"File Name: {fileNameWithoutExtension}");
        }
    }

    public void PrintProfile()
    {
        try
        {
            string loadProfile = inputField_profileCheck.text;

            if (loadProfile == "delAll")
            {
                SaveSystem.DeleteAllFiles(loadProfile);
                return;
            }

            PlayerData player = SaveSystem.LoadPlayer(loadProfile);
            Debug.Log($"Read Profile From save; Name: {player.playerName} Age: {player.playerAge} TP Highscore {player.profile_TP_TotalScore}");

            Debug.Log($"SAVE FILE: {player.playerName} TP: Highest Level {player.profile_TP_Level + 1}\n score lvl-1: {player.profile_TP_Level_1_Score}" +
                        $", score lvl-2: {player.profile_TP_Level_2_Score}, score lvl-3: {player.profile_TP_Level_3_Score}");

            Debug.Log($"SI LEVEL DATA~\n [Clears]\nLevel 1: {player.stage_1_cleared}, Level 2: {player.stage_2_cleared}, Level 3: {player.stage_3_cleared}");
            Debug.Log($"TP LEVEL DATA~\n [Clears]\nLevel 1: {player.stage_SI_1_cleared}, Level 2: {player.stage_SI_2_cleared}, Level 3: {player.stage_SI_3_cleared}");
            // scanned species
            Debug.Log($"Species Scanned COUNT: {player.scannedSpeciesList.Count}");

            foreach (Species species in player.scannedSpeciesList)
            {
                Debug.Log($"Scanned Species is: {species.speciesName}");
            }
        }
        catch (System.Exception)
        {
            Debug.Log($"No existing save profile");
            throw;
        }
    }
    #endregion
}

