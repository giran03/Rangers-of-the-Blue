using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrashPickingHandler : MonoBehaviour
{
    [SerializeField] GameObject netPrefab;
    [SerializeField] Vector3 prefabOffset;

    [Header("Levels")]
    [SerializeField] GameObject shoreLevel;
    [SerializeField] GameObject sandLevel;
    [SerializeField] GameObject coralLevel;

    GameObject netObj;
    GameObject levelPrefab;
    String selectedLevel;

    public ARTrackedImageManager aRTrackedImageManager;

    private void OnEnable()
    {
        aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
        aRTrackedImageManager.trackedImagesChanged += OnImageChange;

        // default level
        levelPrefab = shoreLevel;
    }

    private void Update() => LevelSelection();

    void ShoreLevel() => levelPrefab = shoreLevel;
    void SandLevel() => levelPrefab = sandLevel;
    void CoralLevel() => levelPrefab = coralLevel;

    void LevelSelection()
    {
        selectedLevel = PlayerPrefs.GetString("SelectedLevel");
        if (selectedLevel == "shoreLevel")
            ShoreLevel();
        else if (selectedLevel == "sandLevel")
            SandLevel();
        else if (selectedLevel == "coralLevel")
            CoralLevel();
    }

    void OnImageChange(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
            SpawnLevel(image);
    }

    void SpawnNet(Transform image)
    {
        netObj = Instantiate(netPrefab, image.transform);
        netObj.transform.position += prefabOffset;
    }

    public void SpawnLevel(ARTrackedImage image)
    {
        SpawnNet(image.transform);
        Instantiate(levelPrefab, image.transform);

        Debug.Log("Level Spawned!");
    }
}
