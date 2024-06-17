using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerAge;
    public int profile_TP_Level;
    public int profile_TP_Level_1_Score;
    public int profile_TP_Level_2_Score;
    public int profile_TP_Level_3_Score;
    public int profile_TP_TotalScore;
    public int profile_SI_Level;
    public int profile_SI_TotalScore;
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

        // SI GAMEMODE
        profile_SI_Level = profile.profile_SI_Level;
        profile_SI_TotalScore = profile.profile_SI_TotalScore;
        scannedSpeciesList = profile.scannedSpeciesList;
    }
}
