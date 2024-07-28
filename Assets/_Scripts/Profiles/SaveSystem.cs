using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Collections.Generic;

public static class SaveSystem
{
    public static string _selectedProfile;
    public static string SelectedProfileName
    {
        get { return _selectedProfile = PlayerPrefs.GetString("SelectedProfile"); }
        set { PlayerPrefs.SetString("SelectedProfile", value); }
    }

    public static void ResetSelectedProfile() => SelectedProfileName = null;

    public static int TP_GetHighestLevel()
    {
        PlayerData data = LoadPlayer(SelectedProfileName);
        Debug.Log($"TP highest level of {data.playerName} is {data.profile_TP_Level}");
        return data.profile_TP_Level;
    }

    public static void SavePlayer(Profile profile)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + $"/{profile.playerName}.fish";
        FileStream stream = new(path, FileMode.Create);

        PlayerData data = new(profile);

        Debug.Log($"Saved Profile for: {profile.playerName}\nSaved in path: {path}");

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveExistingPlayer(PlayerData newData)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + $"/{newData.playerName}.fish";
        FileStream stream = new(path, FileMode.Truncate);

        Debug.Log($"UPDATED PROFILE for: {newData.playerName}\nSaved in path: {path}");

        formatter.Serialize(stream, newData);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string searchString)
    {
        // Use Directory.EnumerateFiles to find matching files
        string[] potentialFiles = Directory.EnumerateFiles(Application.persistentDataPath, $"{searchString}.fish")
        .Where(path => path.Contains(searchString)).ToArray();

        if (potentialFiles.Length == 0)
        {
            Debug.LogError($"No save file found matching search string: {searchString}");
            return null;
        }

        // If multiple files match, consider additional logic (e.g., latest modification date)
        string filePath = potentialFiles[0]; // Use the first matching file for now

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(filePath, FileMode.Open);

            try
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                Debug.Log($"!!!Loaded player data for {data.playerName}");
                stream.Close();
                return data;
            }
            catch (System.Exception)
            {
                Debug.LogError("Error loading player data:");
                return null;
            }
        }
        else
        {
            Debug.LogError($"Save file not found: {filePath}");
            return null;
        }
    }

    // public static void SaveCurrentProfile(string profile)
    // {
    //     // string profile = PlayerPrefs.GetString("SelectedProfile");
    //     SavePlayer(GetProfileByName(profile));
    //     Debug.Log($"SAVED CURRENT PROFILE FOR {profile}!");
    // }


    public static List<PlayerData> GetPlayerData()
    {
        List<PlayerData> playerDataList = new();
        string[] potentialFiles = Directory.EnumerateFiles(Application.persistentDataPath, "*.fish").ToArray();

        if (potentialFiles.Length == 0)
        {
            return playerDataList;
        }

        foreach (string filePath in potentialFiles)
        {
            if (File.Exists(filePath))
            {
                BinaryFormatter formatter = new();
                FileStream stream = new(filePath, FileMode.Open);
                try
                {
                    PlayerData data = formatter.Deserialize(stream) as PlayerData;
                    playerDataList.Add(data);
                    stream.Close();
                }
                catch (System.Exception)
                {
                    Debug.LogError($"Error loading player data from {filePath}");
                    throw;
                }
            }
            else
            {
                Debug.LogError($"Save file not found: {filePath}");
            }
        }

        return playerDataList;
    }

    public static void DeleteAllFiles(string confirmationKey)
    {
        if (confirmationKey == "deleteAll")
        {
            string[] potentialFiles = Directory.EnumerateFiles(Application.persistentDataPath, "*.fish").ToArray();

            if (potentialFiles.Length == 0)
            {
                Debug.Log("No .fish files found in the specified directory.");
                return;
            }
            Debug.Log("Found Save Files:");
            foreach (string filePath in potentialFiles)
            {
                Debug.Log($"- {Path.GetFileNameWithoutExtension(filePath)}");
                File.Delete(filePath);
            }
            ProfilesHandler profilesHandler = new();
            profilesHandler.UpdateOptions();
        }
    }
}
