using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class IndexHandler : MonoBehaviour
{
    [SerializeField] Transform entryContainer;
    [SerializeField] Transform entryTemplate;
    [SerializeField] float templateHeight = 150f;
    List<Transform> speciesEntryTransformList;
    List<Transform> lastSaved_EntryTransformList = new();
    List<Species> speciesDataList;

    public static IndexHandler Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        // entryContainer = transform.Find("ScannedSpeciesContainer");
        // entryTemplate = entryContainer.Find("ScannedSpeciesTemplate");
        entryTemplate.gameObject.SetActive(false);
    }

    // USED BY INDEX BUTTON IN MENU POP-UP
    public void RefreshIndex()
    {
        if (lastSaved_EntryTransformList != null)
        {
            foreach (Transform item in lastSaved_EntryTransformList)
            {
                Destroy(item.gameObject);
            }
            lastSaved_EntryTransformList.Clear();
        }

        speciesEntryTransformList = new();

        if (SI_Manager.Instance.ScannedSpeciesCollection != null)
        {

            foreach (Species data in SI_Manager.Instance.ScannedSpeciesCollection)
                CreateSpeciesEntryTransform(data, entryContainer, speciesEntryTransformList);
        }
        lastSaved_EntryTransformList = speciesEntryTransformList;

        Debug.Log($"Refreshed index screen!");
    }

    public void AddSpeciesEntry(Species species)
    {
        // create species entry
        SpeciesEntry speciesEntry = new()
        {
            speciesName = species.speciesName,
            scientificName = species.scientificName,
            conservationStatus = species.conservationStatus,
            kingdom = species.kingdom
        };

        // load saved species
        string jsonString = PlayerPrefs.GetString("speciesIndexTable");
        SpeciesScanned speciesScanned = JsonUtility.FromJson<SpeciesScanned>(jsonString);

        // add entry to species
        speciesScanned.speciesEntriesList.Add(speciesEntry);

        // save and update species
        string json = JsonUtility.ToJson(speciesScanned);
        PlayerPrefs.SetString("speciesIndexTable", json);
        PlayerPrefs.Save();
    }

    private void CreateSpeciesEntryTransform(Species data, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight) * transformList.Count;
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("SpeciesImage").GetComponent<UnityEngine.UI.Image>().sprite = GetSpeciesImage(data.speciesName);
        entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().SetText($"Species Name: {data.speciesName}");
        entryTransform.Find("ScientificName").GetComponent<TextMeshProUGUI>().SetText($"Scientific Name: {data.scientificName}");
        entryTransform.Find("ConservationStatus").GetComponent<TextMeshProUGUI>().SetText($"Conservation Status: {data.conservationStatus}");
        entryTransform.Find("Kingdom").GetComponent<TextMeshProUGUI>().SetText($"Kingdom: {data.kingdom}");

        entryTransform.Find("Background").gameObject.SetActive(transformList.Count % 2 == 1); // Assuming even/odd row background logic

        transformList.Add(entryTransform);
    }

    public Sprite GetSpeciesImage(string speciesNameToFind)
    {
        var speciesIndex = SI_Manager.Instance.speciesIndex_Array.FirstOrDefault(si => si.speciesIndexName == speciesNameToFind);

        return speciesIndex?.speciesImg; // null propagation
    }

    private class SpeciesScanned
    {
        public List<SpeciesEntry> speciesEntriesList;
    }

    [System.Serializable]
    private class SpeciesEntry
    {
        public string speciesName;
        public string scientificName;
        public string conservationStatus;
        public string kingdom;
    }
}
