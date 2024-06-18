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

        // switch (PlayerPrefs.GetInt("TP_SelectedLevel"))
        // {
        //     case 0:
        //         label_LVL1_Highscore.SetText($"{playerData.profile_TP_Level_1_Score}");
        //         break;
        //     case 1:
        //         label_LVL2_Highscore.SetText($"{playerData.profile_TP_Level_2_Score}");
        //         break;
        //     case 2:
        //         label_LVL3_Highscore.SetText($"{playerData.profile_TP_Level_3_Score}");
        //         break;
        // }
        label_LVL1_Highscore.SetText($"{playerData.profile_TP_Level_1_Score}");
        label_LVL2_Highscore.SetText($"{playerData.profile_TP_Level_2_Score}");
        label_LVL3_Highscore.SetText($"{playerData.profile_TP_Level_3_Score}");

        if (playerData.profile_TP_Level_1_Score > 0)
            checkmark_LVL1.SetActive(true);
        if (playerData.profile_TP_Level_2_Score > 0)
            checkmark_LVL2.SetActive(true);
        if (playerData.profile_TP_Level_3_Score > 0)
            checkmark_LVL3.SetActive(true);
    }

    public void Button_LoadDataInfo()
    {
        if (isDataLoaded) return;

        playerData = Profile.Instance.LoadPlayer(SaveSystem.SelectedProfileName);
        isDataLoaded = true;
    }

}
