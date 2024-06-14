using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfilesHandler : MonoBehaviour
{
    [SerializeField] InputField inputField_name;
    [SerializeField] InputField inputField_age;
    [SerializeField] Dropdown dropdown_Profiles;
    string _name;
    int _age;
    string selectedProfile;
    bool isCreatingProfile = true;

    [SerializeField] InputField inputField_profileCheck;

    private void Start()
    {
        UpdateOptions();
    }

    private void Update()
    {
        try
        {
            _name = inputField_name.text;
            _age = System.Convert.ToInt32(inputField_age.text);
        }
        catch (System.Exception)
        {
            Debug.Log("Please fill up the forms correctly~");
        }

        if (!isCreatingProfile)
            GetDropdownValue();
    }

    void UpdateOptions()
    {
        List<string> fileNames = GetPlayerFileNamesWithoutExtension();

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
        selectedProfile = "";
    }
    public void Button_ExistingProfile() => isCreatingProfile = false;

    public void GetDropdownValue()
    {
        selectedProfile = dropdown_Profiles.options[dropdown_Profiles.value].text;
        Debug.Log($"Dropdown Selected: {selectedProfile}");
    }

    public void Button_CreateProfile()
    {
        Profile.Instance.playerName = _name;
        Profile.Instance.playerAge = _age;
        Profile.Instance.SavePlayer();

        UpdateOptions();
        Debug.Log($"NEW PROFILE CREATED! name: {Profile.Instance.playerName} , age: {Profile.Instance.playerAge}");
    }

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

    public void PrintProfile()
    {
        try
        {
            string loadProfile = inputField_profileCheck.text;

            PlayerData player = Profile.Instance.LoadPlayer(loadProfile);
            Debug.Log($"Read Profile From save; Name: {player.playerName} Age: {player.playerAge} TP Highscore {player.profile_TP_TotalScore}");
        }
        catch (System.Exception)
        {
            Debug.Log($"No existing save profile");
            throw;
        }
    }
}

