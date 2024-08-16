using UnityEngine;

public class ReloadObjects : MonoBehaviour
{
    public GameObject[] gameObjects;
    public static PlayerData ProfileToCheck { get; set; }

    private int currentGameObjectIndex = 0;

    /*
    Button_Reload_UI() is USED BY:
    Back Button - Both Gamemode Level Selection Display
    */
    public void Button_Reload_UI()
    {
        GameSceneManager.Instance.RestartLevel();
    }

    void DisableAllGameObjects()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(false);
        }
    }

    void EnableNextGameObject()
    {
        // Disable the current GameObject
        gameObjects[currentGameObjectIndex].SetActive(false);

        // Increment the index, looping back to 0 if necessary
        currentGameObjectIndex = (currentGameObjectIndex + 1) % gameObjects.Length;

        // Enable the next GameObject
        gameObjects[currentGameObjectIndex].SetActive(true);
    }
}
