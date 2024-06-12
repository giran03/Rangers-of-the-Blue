using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class TrashpickingGameplayManager : MonoBehaviour
{
    public static TrashpickingGameplayManager Instance;

    [Header("Configs")]
    /// Game Levels:
    /// 0 - Shore,
    /// 1 - Sand,
    /// 2 - Coral
    [SerializeField] GameObject[] levelCollection;
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
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel");
        Debug.Log("selectedLevel: " + selectedLevel);
        currentSelectedLevel = selectedLevel;

        levelCollection[selectedLevel].SetActive(true);
    }
}
