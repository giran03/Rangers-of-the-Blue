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

    // Update data method to update the profile with new values
    public void UpdateData(PlayerData newData)
    {
        Debug.Log("Data being updated!~");
        PlayerData playerData = newData;
        PlayerData oldData = SaveSystem.LoadPlayer(newData.playerName);

        //TODO: Add high score total and score checks
        // TP GAMEMODE
        oldData.profile_TP_Level = playerData.profile_TP_Level;

        // SCORE CHECKS
        if (oldData.profile_TP_Level_1_Score < playerData.profile_TP_Level_1_Score)
            oldData.profile_TP_Level_1_Score = playerData.profile_TP_Level_1_Score;
        else Debug.Log($"KEEPING OLD SCORE");

        if (oldData.profile_TP_Level_2_Score < playerData.profile_TP_Level_2_Score)
            oldData.profile_TP_Level_2_Score = playerData.profile_TP_Level_2_Score;
        else Debug.Log($"KEEPING OLD SCORE");

        if (oldData.profile_TP_Level_3_Score < playerData.profile_TP_Level_3_Score)
            oldData.profile_TP_Level_3_Score = playerData.profile_TP_Level_3_Score;
        else Debug.Log($"KEEPING OLD SCORE");

        oldData.profile_TP_TotalScore = oldData.profile_TP_Level_1_Score + oldData.profile_TP_Level_2_Score + oldData.profile_TP_Level_3_Score; // Update total score if needed

        // SI GAMEMODE
        oldData.profile_SI_Level = playerData.profile_SI_Level;
        oldData.profile_SI_TotalScore = playerData.profile_SI_TotalScore; // Update total score if needed
        oldData.scannedSpeciesList = playerData.scannedSpeciesList; // Update scanned species list if needed

        SaveSystem.SaveExistingPlayer(oldData);
        Debug.Log($"Sample of new data scoring : {oldData.profile_TP_Level_1_Score}");
    }
}
