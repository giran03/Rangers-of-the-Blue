using System.Collections.Generic;

[System.Serializable]
public class PlayerData
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

    public PlayerData(Profile profile)
    {
        playerName = profile.playerName;
        playerAge = profile.playerAge;

        // TP GAMEMODE
        profile_TP_Level = profile.profile_TP_Level;
        profile_TP_Level_1_Score = profile.profile_TP_Level_1_Score;
        profile_TP_Level_2_Score = profile.profile_TP_Level_2_Score;
        profile_TP_Level_3_Score = profile.profile_TP_Level_3_Score;
        profile_TP_TotalScore = profile.profile_TP_TotalScore;

        stage_1_cleared = profile.stage_1_cleared;
        stage_2_cleared = profile.stage_2_cleared;
        stage_3_cleared = profile.stage_3_cleared;

        // SI GAMEMODE
        profile_SI_Level = profile.profile_SI_Level;
        profile_SI_Level_1_Score = profile.profile_SI_Level_1_Score;
        profile_SI_Level_2_Score = profile.profile_SI_Level_2_Score;
        profile_SI_Level_3_Score = profile.profile_SI_Level_3_Score;
        profile_SI_TotalScore = profile.profile_SI_TotalScore;
        scannedSpeciesList = profile.scannedSpeciesList;

        stage_SI_1_cleared = profile.stage_SI_1_cleared;
        stage_SI_2_cleared = profile.stage_SI_2_cleared;
        stage_SI_3_cleared = profile.stage_SI_3_cleared;
    }
    public PlayerData()
    { }
}
