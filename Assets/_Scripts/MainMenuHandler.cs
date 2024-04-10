using Lean.Transition;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public static MainMenuHandler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    [SerializeField] GameObject mainMenuDisplay;
    [SerializeField] GameObject gameModeSelectDisplay;

    public void LevelSelect()
    {
        FlipState(gameModeSelectDisplay);
        FlipState(mainMenuDisplay);
    }

    public void ReturnMainMenu()
    {
        FlipState(mainMenuDisplay);
    }

    public void FlipState(GameObject gameObject) => gameObject.SetActive(!gameObject.activeSelf);
}
