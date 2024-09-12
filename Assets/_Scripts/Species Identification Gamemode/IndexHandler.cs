using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        GameSceneManager.Instance.PauseGame();
    }

    private void CreateSpeciesEntryTransform(Species data, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight) * transformList.Count;
        entryTransform.gameObject.SetActive(true);

        // ADD BUTTON LISTENER
        Button entryButton = entryTransform.GetComponent<Button>();
        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(() => Add_Button_SFX(data.speciesName));

        entryTransform.Find("SpeciesImage").GetComponent<Image>().sprite = GetSpeciesImage(data.speciesName);
        entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().SetText($"{data.speciesName}");
        entryTransform.Find("ScientificName").GetComponent<TextMeshProUGUI>().SetText($"{data.scientificName}");
        entryTransform.Find("ConservationStatus").GetComponent<TextMeshProUGUI>().SetText($"{data.conservationStatus}");
        entryTransform.Find("Family").GetComponent<TextMeshProUGUI>().SetText($"{data.kingdom}");

        entryTransform.Find("Background").gameObject.SetActive(transformList.Count % 2 == 1); // Assuming even/odd row background logic

        transformList.Add(entryTransform);
    }

    void Add_Button_SFX(string buttonSpecies)
    {
        AudioManager.Instance.PlaySFX(buttonSpecies);
    }

    public Sprite GetSpeciesImage(string speciesNameToFind)
    {
        var speciesIndex = SI_Manager.Instance.speciesIndex_Array.FirstOrDefault(si => si.speciesIndexName == speciesNameToFind);

        return speciesIndex?.speciesImg;
    }
}
