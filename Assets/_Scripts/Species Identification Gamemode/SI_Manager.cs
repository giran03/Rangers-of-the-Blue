using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SI_Manager : MonoBehaviour
{
    [Header("Species Configs")]
    [SerializeField] Species[] _species;
    [SerializeField] GameObject dispaly_scanInfoBox;
    [SerializeField] TMP_Text label_speciesName;
    [SerializeField] TMP_Text label_scientificName;
    [SerializeField] TMP_Text label_conservationStatus;
    [SerializeField] TMP_Text label_kingdom;

    List<Species> ScannedSpeciesCollection = new();

    Vector3 screenCenter = new(0.5f, 0.5f, 0f);

    private void Start()
    {
        dispaly_scanInfoBox.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Species"))
            {
                GameObject species = hit.collider.gameObject;

                if (species.TryGetComponent<SpeciesName>(out SpeciesName speciesName))
                {
                    dispaly_scanInfoBox.SetActive(true);
                    GetSpeciesInfo(speciesName.speciesName);
                }
            }
        }
        else
        {
            dispaly_scanInfoBox.SetActive(false);
            Debug.Log("No species beings scanned!");
        }
    }

    void GetSpeciesInfo(string speciesName)
    {
        Species species = Array.Find(_species, x => x.name == speciesName);
        if (species != null)
        {
            Debug.Log("Im being called from get species info");
            Debug.Log($"Species name is: {species.name}\nSpecies Scientific Name: {species.scientificName}");

            label_speciesName.SetText($"Name: {species.name}");
            label_scientificName.SetText($"Scientific Name: {species.scientificName}");
            label_conservationStatus.SetText($"Conservation Status: {species.conservationStatus}");
            label_kingdom.SetText($"Kingdom: {species.kingdom}");

            if (!ScannedSpeciesCollection.Contains(species))
                ScannedSpeciesCollection.Add(species);

            SpeciesScoring(species.name);
        }
        else
            Debug.Log("Cant find species data");
    }

    void SpeciesScoring(string speciesName)
    {
        switch (speciesName)
        {
            // common species
            case "Bangus":
                Debug.Log("Scanned Bangus~\n +5 points");
                break;
            case "Tilapia":
                Debug.Log("Scanned Tilapia~\n +5 points");
                break;

            // endangered species
            case "Green Sea Turtle":
                Debug.Log("Scanned Green Sea Turtle~\n +75 points");
                break;
        }
        
        Debug.Log($"The Scanned Species Collection Count is {ScannedSpeciesCollection.Count}");
    }
}

[Serializable]
public class Species
{
    public string name;
    public string scientificName;
    public string conservationStatus;
    public string kingdom;
}