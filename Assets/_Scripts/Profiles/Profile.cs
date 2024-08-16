using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    public string playerName;
    public int playerAge;

    // TP GAMEMODE
    public int profile_TP_Level;
    public int profile_TP_Level_1_Score;
    public int profile_TP_Level_2_Score;
    public int profile_TP_Level_3_Score;
    public int profile_TP_TotalScore;

    // SI GAMEMODE
    public int profile_SI_Level;
    public int profile_SI_Level_1_Score;
    public int profile_SI_Level_2_Score;
    public int profile_SI_Level_3_Score;
    public int profile_SI_TotalScore;

    // TP level clear
    public bool stage_1_cleared;
    public bool stage_2_cleared;
    public bool stage_3_cleared;

    // SI level clear
    public bool stage_SI_1_cleared;
    public bool stage_SI_2_cleared;
    public bool stage_SI_3_cleared;
    public List<Species> scannedSpeciesList;

    public static Profile Instance;

    public Profile(PlayerData playerData)
    {
        playerName = playerData.playerName;
        playerAge = playerData.playerAge;
        // TP GAMEMODE
        profile_TP_Level = playerData.profile_TP_Level;
        profile_TP_Level_1_Score = playerData.profile_TP_Level_1_Score;
        profile_TP_Level_2_Score = playerData.profile_TP_Level_2_Score;
        profile_TP_Level_3_Score = playerData.profile_TP_Level_3_Score;
        profile_TP_TotalScore = playerData.profile_TP_TotalScore;

        // SI GAMEMODE
        profile_SI_Level = playerData.profile_SI_Level;
        profile_SI_Level_1_Score = playerData.profile_SI_Level_1_Score;
        profile_SI_Level_2_Score = playerData.profile_SI_Level_2_Score;
        profile_SI_Level_3_Score = playerData.profile_SI_Level_3_Score;
        profile_SI_TotalScore = playerData.profile_SI_TotalScore;
        scannedSpeciesList = playerData.scannedSpeciesList;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void PrintScannedSpecies()
    {
        PlayerData playerData = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);

        foreach (Species species in playerData.scannedSpeciesList)
        {
            Debug.Log(species.speciesName);
            Debug.Log(species.scientificName);
            Debug.Log(species.conservationStatus);
        }
    }

    public PlayerData LoadPlayer(string searchString)
    {
        PlayerData data = SaveSystem.LoadPlayer(searchString);

        playerName = data.playerName;
        playerAge = data.playerAge;

        // TP GAMEMODE
        profile_TP_Level = data.profile_TP_Level;
        profile_TP_TotalScore = data.profile_TP_TotalScore;

        // SI GAMEMODE
        profile_SI_Level = data.profile_SI_Level;
        profile_SI_TotalScore = data.profile_SI_TotalScore;
        scannedSpeciesList = data.scannedSpeciesList;

        return data;
    }

    // update profile with new values
    public void UpdateData(PlayerData newData)
    {
        Debug.Log("Data being updated!~");
        PlayerData oldData = LoadPlayer(SaveSystem.SelectedProfileName);

        // TP GAMEMODE
        oldData.profile_TP_Level = newData.profile_TP_Level;

        // TP SCORE CHECKS
        if (oldData.profile_TP_Level_1_Score < newData.profile_TP_Level_1_Score)
            oldData.profile_TP_Level_1_Score = newData.profile_TP_Level_1_Score;
        else Debug.Log($"TP | KEEPING OLD SCORE FOR LEVEL 1 {oldData.profile_TP_Level_1_Score}");

        if (oldData.profile_TP_Level_2_Score < newData.profile_TP_Level_2_Score)
            oldData.profile_TP_Level_2_Score = newData.profile_TP_Level_2_Score;
        else Debug.Log($"TP | KEEPING OLD SCORE FOR LEVEL 2 {oldData.profile_TP_Level_2_Score}");

        if (oldData.profile_TP_Level_3_Score < newData.profile_TP_Level_3_Score)
            oldData.profile_TP_Level_3_Score = newData.profile_TP_Level_3_Score;
        else Debug.Log($"TP | KEEPING OLD SCORE FOR LEVEL 3 of {oldData.profile_TP_Level_3_Score}");

        oldData.profile_TP_TotalScore = oldData.profile_TP_Level_1_Score + oldData.profile_TP_Level_2_Score + oldData.profile_TP_Level_3_Score;

        // TP level clears 
        oldData.stage_1_cleared = newData.stage_1_cleared;
        oldData.stage_2_cleared = newData.stage_2_cleared;
        oldData.stage_3_cleared = newData.stage_3_cleared;

        SaveSystem.SaveExistingPlayer(oldData);
    }

    public void UpdateData_SI(PlayerData newData)
    {
        Debug.Log("Data being updated!~");
        PlayerData oldData = LoadPlayer(SaveSystem.SelectedProfileName);

        // SI GAMEMODE
        oldData.profile_SI_Level = newData.profile_SI_Level;

        // SI SCORE CHECKS
        if (oldData.profile_SI_Level_1_Score < newData.profile_SI_Level_1_Score)
            oldData.profile_SI_Level_1_Score = newData.profile_SI_Level_1_Score;
        else Debug.Log($"SI | KEEPING OLD SCORE FOR LEVEL 1 {oldData.profile_SI_Level_1_Score}");

        if (oldData.profile_SI_Level_2_Score < newData.profile_SI_Level_2_Score)
            oldData.profile_SI_Level_2_Score = newData.profile_SI_Level_2_Score;
        else Debug.Log($"SI | KEEPING OLD SCORE FOR LEVEL 2 {oldData.profile_SI_Level_2_Score}");

        if (oldData.profile_SI_Level_3_Score < newData.profile_SI_Level_3_Score)
            oldData.profile_SI_Level_3_Score = newData.profile_SI_Level_3_Score;
        else Debug.Log($"SI | KEEPING OLD SCORE FOR LEVEL 3 of {oldData.profile_SI_Level_3_Score}");

        oldData.profile_SI_TotalScore = oldData.profile_SI_Level_1_Score + oldData.profile_SI_Level_2_Score + oldData.profile_SI_Level_3_Score;

        // scanned species list checks
        if (oldData.scannedSpeciesList != null)
        {
            if (oldData.scannedSpeciesList.Count < newData.scannedSpeciesList.Count)
                oldData.scannedSpeciesList = newData.scannedSpeciesList;
        }
        else
            oldData.scannedSpeciesList = newData.scannedSpeciesList;

        // SI level clears 
        oldData.stage_SI_1_cleared = newData.stage_SI_1_cleared;
        oldData.stage_SI_2_cleared = newData.stage_SI_2_cleared;
        oldData.stage_SI_3_cleared = newData.stage_SI_3_cleared;

        SaveSystem.SaveExistingPlayer(oldData);
    }
}
