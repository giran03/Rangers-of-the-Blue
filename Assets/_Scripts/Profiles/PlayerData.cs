[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerAge;
    public int profile_TP_Level;
    public int profile_TP_TotalScore;
    public int profile_SI_Level;
    public int profile_SI_TotalScore;

    public PlayerData(Profile profile)
    {
        playerName = profile.playerName;
        playerAge = profile.playerAge;
    }
}
