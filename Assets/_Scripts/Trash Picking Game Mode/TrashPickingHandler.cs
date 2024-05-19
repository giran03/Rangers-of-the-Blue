using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrashPickingHandler : MonoBehaviour
{
    [SerializeField] GameObject netPrefab;
    [SerializeField] Vector3 prefabOffset;

    GameObject netObj;
    ARTrackedImageManager aRTrackedImageManager;
    ARTrackedImage currentImage;
    bool hasSpawned;

    void Awake()
    {
        aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        aRTrackedImageManager.trackedImagesChanged += OnImageChange;
    }

    void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= OnImageChange;
        hasSpawned = false;
    }

    private void FixedUpdate()
    {
        if (currentImage != null && !hasSpawned)
        {
            netObj = Instantiate(netPrefab, currentImage.transform);
            netObj.transform.position += prefabOffset;
            hasSpawned =true;
        }
        Debug.Log("hasSpawned: " + hasSpawned);
    }

    void OnImageChange(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            currentImage = image;
            Debug.Log("Level Spawned!");
        }

        foreach (ARTrackedImage image in obj.updated)
        {
            aRTrackedImageManager.trackedImagePrefab.transform.rotation = Quaternion.identity;
        }

        foreach (ARTrackedImage image in obj.removed)
        {
            currentImage = null;
            hasSpawned = false;
        }
    }
}
