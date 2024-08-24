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
    [SerializeField] GameObject[] startDialogues;
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
        currentSelectedLevel = selectedLevel;

        foreach (GameObject gameObject in startDialogues)
            gameObject.SetActive(false);

        // skip level 1
        if (currentSelectedLevel != 0)
            startDialogues[currentSelectedLevel].SetActive(true);
    }

    public int GetLevelTimer()
    {
        return levelTimers[selectedLevel];
    }

    public void NextLevel(int level)
    {
        selectedLevel = level;

        GetLevelTimer();
        GameSceneManager.Instance.RestartLevel();
    }
}
