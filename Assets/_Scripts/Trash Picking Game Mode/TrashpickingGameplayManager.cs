using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashpickingGameplayManager : MonoBehaviour
{
    [Header("Configs")]
    /// Ascending levels:
    /// 0 - Shore,
    /// 1 - Sand,
    /// 2 - Coral
    [SerializeField] GameObject[] levelCollection;
    int selectedLevel;

    private void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel");
        Debug.Log("selectedLevel: " + selectedLevel);

        levelCollection[selectedLevel].SetActive(true);
    }
}
