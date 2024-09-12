using UnityEngine;

public class TrashpickingGameplayManager : MonoBehaviour
{
    public static TrashpickingGameplayManager Instance;

    [Header("Configs")]
    /// Game Levels:
    /// 0 - Beach,
    /// 1 - Shallow,
    /// 2 - Coral
    [SerializeField] GameObject[] levelCollection;
    [SerializeField] int[] levelTimers;
    int selectedLevel;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("TP_SelectedLevel");

        levelCollection[selectedLevel].SetActive(true);
        Debug.Log($"Player prefs TP Level Selected: {selectedLevel}");
    }

    public int GetLevelTimer()
    {
        return levelTimers[selectedLevel];
    }

    public void NextLevel(int level)
    {
        if (level == 0) return;

        levelCollection[level - 1].SetActive(false);
        levelCollection[level].SetActive(true);
    }
}
