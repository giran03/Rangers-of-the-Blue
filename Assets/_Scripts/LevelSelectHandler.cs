using System.Collections;
using System.Collections.Generic;
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
    }

    public void SandCleaningTapped()
    {
        Debug.Log("Description of Sand Cleaning");
        MainMenuHandler.Instance.FlipState(sandDescription);
        shoreDescription.SetActive(false);
        coralDescription.SetActive(false);
    }

    public void CoralCleaningTapped()
    {
        Debug.Log("Description of Coral Cleaning");
        MainMenuHandler.Instance.FlipState(coralDescription);
        shoreDescription.SetActive(false);
        sandDescription.SetActive(false);
    }

    public void ShoreStart()
    {
        GameSceneManager.Instance.GoToScene("_TrashPickingGamemode");
        PlayerPrefs.SetString("SelectedLevel", "shoreLevel");
        Debug.Log("Shore Cleaning Start!");
    }

    public void SandStart()
    {
        GameSceneManager.Instance.GoToScene("_TrashPickingGamemode");
        PlayerPrefs.SetString("SelectedLevel", "sandLevel");
        Debug.Log("Sand Cleaning Start!");
    }

    public void CoralStart()
    {
        GameSceneManager.Instance.GoToScene("_TrashPickingGamemode");
        PlayerPrefs.SetString("SelectedLevel", "coralLevel");
        Debug.Log("Coral Cleaning Start!");
    }
}
