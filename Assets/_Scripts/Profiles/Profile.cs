using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviour
{
    public string playerName;
    public int playerAge;
    public int profile_TP_Level;
    public int profile_TP_TotalScore;
    public int profile_TP_Level_1_Score;
    public int profile_TP_Level_2_Score;
    public int profile_TP_Level_3_Score;
    public int profile_SI_Level;
    public int profile_SI_TotalScore;
    
    // level clear
    public bool stage_1_cleared;
    public bool stage_2_cleared;
    public bool stage_3_cleared;
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

        // SCORE CHECKS
        if (oldData.profile_TP_Level_1_Score < newData.profile_TP_Level_1_Score)
            oldData.profile_TP_Level_1_Score = newData.profile_TP_Level_1_Score;
        else Debug.Log($"KEEPING OLD SCORE FOR LEVEL 1 {oldData.profile_TP_Level_1_Score}");

        if (oldData.profile_TP_Level_2_Score < newData.profile_TP_Level_2_Score)
            oldData.profile_TP_Level_2_Score = newData.profile_TP_Level_2_Score;
        else Debug.Log($"KEEPING OLD SCORE FOR LEVEL 2 {oldData.profile_TP_Level_2_Score}");

        if (oldData.profile_TP_Level_3_Score < newData.profile_TP_Level_3_Score)
            oldData.profile_TP_Level_3_Score = newData.profile_TP_Level_3_Score;
        else Debug.Log($"KEEPING OLD SCORE FOR LEVEL 3 of {oldData.profile_TP_Level_3_Score}");

        oldData.profile_TP_TotalScore = oldData.profile_TP_Level_1_Score + oldData.profile_TP_Level_2_Score + oldData.profile_TP_Level_3_Score; // Update total score if needed


        // SI GAMEMODE
        oldData.profile_SI_Level = newData.profile_SI_Level;
        
        oldData.profile_SI_TotalScore = newData.profile_SI_TotalScore; // Update total score if needed
        oldData.scannedSpeciesList = newData.scannedSpeciesList; // Update scanned species list if needed

        // level clears TP GAMEMODE
        oldData.stage_1_cleared = newData.stage_1_cleared;
        oldData.stage_2_cleared = newData.stage_2_cleared;
        oldData.stage_3_cleared = newData.stage_3_cleared;

        SaveSystem.SaveExistingPlayer(oldData);
    }
}
