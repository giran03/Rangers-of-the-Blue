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

    PlayerData playerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateOptions();
        dialogue_SI_Unlocked.SetActive(false);
    }

    private void Update()
    {
        if (!isCreatingProfile)
            GetDropdownValue();

        try
        {

            if (inputField_name.text != "" || inputField_name.text != null)
                _name = inputField_name.text;

            // int age_input = System.Convert.ToInt32(inputField_age.text);

            // if (age_input <= 0 || age_input >= 99)
            //     _age = age_input;

            _age = 20;
        }
        catch (System.Exception)
        {
            Debug.Log("Please fill up the forms correctly~");
            // inputField_age.text = "";
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

    public void Button_Creating_NewProfile()
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
        if (inputField_name.text == "" || inputField_name.text == null) return;

        /*
        if (inputField_name.text == "" || inputField_name.text == null
        || inputField_age.text == "" || inputField_age.text == null) return;
        */

        Profile.Instance.playerName = _name;
        Profile.Instance.playerAge = _age;
        Profile.Instance.SavePlayer();
        selectedProfile = _name;

        inputField_name.text = "";
        // inputField_age.text = "";

        Button_SelectThisProfile(selectedProfile);
        UpdateOptions();

        Debug.Log($"NEW PROFILE CREATED! name: {Profile.Instance.playerName} , age: {Profile.Instance.playerAge}");

        DisplayTransition(createButtonTransitionDisplay);
    }

    public void Button_NextButton()
    {
        if (selectedProfile == null)
        {
            Debug.Log($"No selected profile!!!");
            return;
        }
        else
            Button_SelectThisProfile(selectedProfile);

        DisplayTransition(nextButtonTransitionDisplay);
    }

    public void Button_SelectThisProfile(string profile)
    {
        SaveSystem.SelectedProfileName = profile;
        playerData = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);
        label_profileName.SetText($"GOODLUCK {playerData.playerName}!");

        RefreshButtons();
    }

    public void ClearProfileSelection()
    {
        SaveSystem.SelectedProfileName = null;
    }

    public void RefreshButtons()
    {
        if (playerData.stage_3_cleared || playerData.stage_SI_1_cleared)
        {
            lockedButton.SetActive(false);
            dialogue_SI_Unlocked.SetActive(true);
        }
        else if (!playerData.stage_SI_1_cleared)
        {
            lockedButton.SetActive(true);
            dialogue_SI_Unlocked.SetActive(false);
        }
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

