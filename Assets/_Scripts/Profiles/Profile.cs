using UnityEngine;

public class Profile : MonoBehaviour
{
    public string playerName;
    public int playerAge;
    public int profile_TP_Level;
    public int profile_TP_TotalScore;
    public int profile_SI_Level;
    public int profile_SI_TotalScore;
    public static Profile Instance;
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
        profile_TP_Level = data.profile_TP_Level;
        profile_TP_TotalScore = data.profile_TP_TotalScore;
        profile_SI_Level = data.profile_SI_Level;
        profile_SI_TotalScore = data.profile_SI_TotalScore;

        return data;
    }
}
