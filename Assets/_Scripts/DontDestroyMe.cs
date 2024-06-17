using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    public static DontDestroyMe Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
