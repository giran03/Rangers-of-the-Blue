using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    string currentScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update() => currentScene = SceneManager.GetActiveScene().name;

    public void GoToScene(string sceneName) => SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void Quit()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        GoToScene("_Menu");
    }

    public string CurrentScene() => currentScene;

    public void PauseGame()
    {
        Debug.Log($"⚠️ GAME PAUSED ⏸️");
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Debug.Log($"⚠️ GAME RESUMED ▶️");
        Time.timeScale = 1f;
    }
}
