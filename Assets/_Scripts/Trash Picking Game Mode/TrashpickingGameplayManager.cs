using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class TrashpickingGameplayManager : MonoBehaviour
{
    public static TrashpickingGameplayManager Instance;

    [Header("Configs")]
    /// Game Levels:
    /// 0 - Beach,
    /// 1 - Shallow,
    /// 2 - Coral
    [SerializeField] GameObject[] levelCollection;
    int selectedLevel = 0;

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
        selectedLevel = PlayerPrefs.GetInt("TP_SelectedLevel");
        Debug.Log("selected TP Level is: " + selectedLevel);
        currentSelectedLevel = selectedLevel;

        levelCollection[selectedLevel].SetActive(true);
    }
}
