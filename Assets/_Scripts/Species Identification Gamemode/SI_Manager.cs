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

    [Header("Timer Config")]
    [SerializeField] GameObject hud_label_timerHasRunOut;
    [SerializeField] TMP_Text hud_label_timerText;
    public float timeRemaining = 10;
    public bool timerIsRunning;

    List<Species> ScannedSpeciesCollection = new();

    Vector3 screenCenter = new(0.5f, 0.5f, 0f);

    private void Start()
    {
        SaveSystem.SelectedProfileName = "cici";
        dispaly_scanInfoBox.SetActive(false);
        Debug.Log($"Currently Selected profile is: {PlayerPrefs.GetString("SelectedProfile")}");

        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
            }
        }

        if (!timerIsRunning) return;

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

    // TIMER
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        hud_label_timerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
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
            // least concern species
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