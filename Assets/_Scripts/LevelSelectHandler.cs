using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    [SerializeField] GameObject shoreDescription;
    [SerializeField] GameObject sandDescription;
    [SerializeField] GameObject coralDescription;

    public void ShoreCleaningTapped()
    {
        Debug.Log("Description of Shore Cleaning");
        MainMenuHandler.Instance.FlipState(shoreDescription);
        sandDescription.SetActive(false);
        coralDescription.SetActive(false);

        PlayerPrefs.SetInt("SelectedLevel", 0);
    }

    public void SandCleaningTapped()
    {
        Debug.Log("Description of Sand Cleaning");
        MainMenuHandler.Instance.FlipState(sandDescription);
        shoreDescription.SetActive(false);
        coralDescription.SetActive(false);

        PlayerPrefs.SetInt("SelectedLevel", 1);
    }

    public void CoralCleaningTapped()
    {
        Debug.Log("Description of Coral Cleaning");
        MainMenuHandler.Instance.FlipState(coralDescription);
        shoreDescription.SetActive(false);
        sandDescription.SetActive(false);

        PlayerPrefs.SetInt("SelectedLevel", 2);
    }
}
