using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    public void TP_Level_1() => PlayerPrefs.SetInt("TP_SelectedLevel", 0);

    public void TP_Level_2() => PlayerPrefs.SetInt("TP_SelectedLevel", 1);

    public void TP_Level_3() => PlayerPrefs.SetInt("TP_SelectedLevel", 2);
}
