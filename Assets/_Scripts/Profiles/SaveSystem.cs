using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class SaveSystem
{
    public static void SavePlayer(Profile profile)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + $"/{profile.playerName}.fish";
        FileStream stream = new(path, FileMode.Create);

        PlayerData data = new(profile);

        Debug.Log($"Saved Profile for: {profile.playerName}");
        Debug.Log($"Saved in path: {path}");

        formatter.Serialize(stream, data);
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
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            try
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
            }
            catch (System.Exception)
            {
                Debug.LogError("Error loading player data:");
                return null;
            }
            finally
            {
                stream.Close();
            }
        }
        else
        {
            Debug.LogError($"Save file not found: {filePath}");
            return null;
        }
    }
}
