using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    [SerializeField]
    GameObject m_BackButton;

    void Start()
    {
        if (Application.CanStreamedLevelBeLoaded(MenuLoader.GetMenuSceneName()))
            m_BackButton.SetActive(true);
    }

    void Update()
    {
        // Handles Android physical back button
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            BackButtonPressed();
    }

    public void BackButtonPressed()
    {
        string menuSceneName = MenuLoader.GetMenuSceneName();
        if (Application.CanStreamedLevelBeLoaded(menuSceneName))
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Single);

        // 🔊 change music
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Menu BGM");
    }
}
