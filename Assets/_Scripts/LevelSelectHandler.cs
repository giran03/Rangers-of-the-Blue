using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectHandler : MonoBehaviour
{
    [SerializeField] GameObject shoreDescription;
    [SerializeField] GameObject sandDescription;
    [SerializeField] GameObject coralDescription;

    [SerializeField] GameObject trashPickupGameHud;

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
        MainMenuHandler.Instance.FlipState(trashPickupGameHud);
        TrashPickingHandler.Instance.ShoreLevel();
        gameObject.SetActive(false);
        Debug.Log("Shore Cleaning Start!");
    }

    public void SandStart()
    {
        MainMenuHandler.Instance.FlipState(trashPickupGameHud);
        TrashPickingHandler.Instance.SandLevel();
        gameObject.SetActive(false);
        Debug.Log("Sand Cleaning Start!");
    }

    public void CoralStart()
    {
        MainMenuHandler.Instance.FlipState(trashPickupGameHud);
        TrashPickingHandler.Instance.CoralLevel();
        gameObject.SetActive(false);
        Debug.Log("Coral Cleaning Start!");
    }
}
