using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrashPickingHandler : MonoBehaviour
{
    public static TrashPickingHandler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    [SerializeField] GameObject netPrefab;
    [SerializeField] Vector3 prefabOffset;

    [Header("Levels")]
    [SerializeField] GameObject shoreLevel;
    [SerializeField] GameObject sandLevel;
    [SerializeField] GameObject coralLevel;

    GameObject netObj;
    GameObject levelPrefab;
    GameObject currentLevel;
    bool levelSpawned;

    public ARTrackedImage currentImage;
    public ARTrackedImageManager aRTrackedImageManager;
    public bool clearLevel;

    private void OnEnable()
    {
        aRTrackedImageManager = GetComponent<ARTrackedImageManager>();

        aRTrackedImageManager.trackedImagesChanged += OnImageChange;
    }

    public void ShoreLevel() => levelPrefab = shoreLevel;
    public void SandLevel() => levelPrefab = sandLevel;
    public void CoralLevel() => levelPrefab = coralLevel;

    void OnImageChange(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            currentImage = image;
        }
        foreach (ARTrackedImage image in obj.updated)
        {
            if (currentImage == image)
            {
                SpawnLevel();
                levelSpawned = true;
                Debug.Log("currentImage is " + currentImage);
            }
        }
    }

    private void Update()
    {
        DestroyCurrentLevel();
    }

    void SpawnNet(Transform image)
    {
        netObj = Instantiate(netPrefab, image.transform);
        netObj.transform.position += prefabOffset;
    }
    void DestroyCurrentLevel()
    {
        if (clearLevel)
        {
            DestroyNet();
            DestroyLevel();
            levelSpawned = false;
            clearLevel = false;
        }
    }

    public void SpawnLevel()
    {
        if (levelSpawned) return;

        SpawnNet(currentImage.transform);
        GameObject levelHolder = Instantiate(levelPrefab, currentImage.transform);
        currentLevel = levelHolder;
        levelSpawned = true;

        Debug.Log("Level Spawned!");
    }
    public void DestroyNet() => Destroy(netObj);

    public void DestroyLevel() => Destroy(currentLevel);
}
