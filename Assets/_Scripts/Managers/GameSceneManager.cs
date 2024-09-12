using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    string currentScene;
    float defaultVolume_music;
    float defaultVolume_sfx;

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

    public void FinishLevel()
    {

        Debug.Log($"⚠️ Returning to gamemode select screen!");
        PlayerPrefs.SetString("gameFinished", "true");
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

    public void MuteGame()
    {
        defaultVolume_music = AudioManager.Instance.currentMusic.volume;
        AudioManager.Instance.currentMusic.volume = 0f;
    }

    public void UnMuteGame()
    {
        AudioManager.Instance.currentMusic.volume = defaultVolume_music;
    }
    public void Toggle_MUSIC() => AudioManager.Instance.musicSource.mute = !AudioManager.Instance.musicSource.mute;


    public void Toggle_SFX() => AudioManager.Instance.sfxSource.mute = !AudioManager.Instance.sfxSource.mute;
}