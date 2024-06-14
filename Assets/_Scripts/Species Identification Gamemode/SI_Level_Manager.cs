using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Manager_Level_Manager : MonoBehaviour
{
    public static SI_Manager_Level_Manager Instance;

    [Header("Configs")]
    /// Game Stages:
    /// 0
    /// 1
    /// 2
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
        selectedLevel = PlayerPrefs.GetInt("SI_SelectedLevel");
        Debug.Log("selected SI Level is: " + selectedLevel);
        currentSelectedLevel = selectedLevel;

        levelCollection[selectedLevel].SetActive(true);
    }
}
