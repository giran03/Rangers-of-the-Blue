using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    string currentScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        QualitySettings.vSyncCount = 1;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
            EnableCursor();
    }

    private void Update() => currentScene = SceneManager.GetActiveScene().name;

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        // string currentScene = SceneManager.GetActiveScene().name;
        // SceneManager.LoadSceneAsync(sceneName);
        // SceneManager.UnloadSceneAsync(currentScene);
        // ARSession session = FindObjectOfType<ARSession>();
        // Debug.Log("session: "+ session);
        // session.Reset();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene("_Game");
    }

    public string CurrentScene() => currentScene;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        EnableCursor();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        DisableCursor();
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}