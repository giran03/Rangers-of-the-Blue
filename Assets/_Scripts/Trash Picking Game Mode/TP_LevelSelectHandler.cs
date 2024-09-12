using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TP_LevelSelectHandler : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] TMP_Text label_LVL1_Highscore;
    [SerializeField] TMP_Text label_LVL2_Highscore;
    [SerializeField] TMP_Text label_LVL3_Highscore;
    // CHECKMARKS
    [SerializeField] GameObject checkmark_LVL1;
    [SerializeField] GameObject checkmark_LVL2;
    [SerializeField] GameObject checkmark_LVL3;

    PlayerData playerData;
    public static bool isDataLoaded;

    private void Update()
    {
        if (!isDataLoaded) return;

        label_LVL1_Highscore.SetText($"{playerData.profile_TP_Level_1_Score}");
        label_LVL2_Highscore.SetText($"{playerData.profile_TP_Level_2_Score}");
        label_LVL3_Highscore.SetText($"{playerData.profile_TP_Level_3_Score}");

        if (playerData.stage_1_cleared)
            checkmark_LVL1.SetActive(true);
        else checkmark_LVL1.SetActive(false);

        if (playerData.stage_2_cleared)
            checkmark_LVL2.SetActive(true);
        else checkmark_LVL2.SetActive(false);

        if (playerData.stage_3_cleared)
            checkmark_LVL3.SetActive(true);
        else checkmark_LVL3.SetActive(false);
    }

    public void Button_LoadDataInfo()
    {
        if (isDataLoaded) return;

        // CHANGED FROM => Profile's load player func ⚠️⚠️⚠️⚠️⚠️⚠️
        playerData = SaveSystem.LoadPlayer(SaveSystem.SelectedProfileName);
        isDataLoaded = true;
    }

    public void Button_CanLoadData() => isDataLoaded = false;
}
