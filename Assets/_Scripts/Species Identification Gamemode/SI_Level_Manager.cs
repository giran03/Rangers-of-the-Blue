using UnityEngine;

public class SI_Level_Manager : MonoBehaviour
{
    public static SI_Level_Manager Instance;

    [Header("Configs")]
    /// Game Stages:
    /// 0
    /// 1
    /// 2
    [SerializeField] int[] levelTimers;
    int selectedLevel;

    public int currentSelectedLevel;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("SI_SelectedLevel");
        Debug.Log("selected SI Level is: " + selectedLevel);
        currentSelectedLevel = selectedLevel;
    }

    public int GetLevelTimer()
    {
        Debug.Log($"RETURNING timer for level {selectedLevel}");
        return levelTimers[selectedLevel];
    }

    public void NextLevel(int level)
    {
        selectedLevel = level;

        GetLevelTimer();
        GameSceneManager.Instance.RestartLevel();
    }
}
