using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    Transform entryContainer;
    Transform entryTemplate;
    float templateHeight = 40f;
    List<Transform> highscoreEntryTransformList;
    List<PlayerData> playerDataList;

    public static LeaderboardHandler Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        RefreshLeaderboards();
    }

    public void RefreshLeaderboards()
    {
        playerDataList = SaveSystem.GetPlayerData(); // Get data for all .fish files

        if (playerDataList != null)
            if (playerDataList.Count > 0)
                for (int i = 0; i < playerDataList.Count; i++) // sort by TP SCORES
                {
                    for (int j = i + 1; j < playerDataList.Count; j++)
                    {
                        if (playerDataList[j].profile_TP_TotalScore > playerDataList[i].profile_TP_TotalScore)
                        {
                            (playerDataList[j], playerDataList[i]) = (playerDataList[i], playerDataList[j]);    // swap
                        }
                    }
                }

        highscoreEntryTransformList = new();
        foreach (PlayerData data in playerDataList)
            CreateHighscoreEntryTransform(data, entryContainer, highscoreEntryTransformList);
    }

    public void AddHighscoreEntry(string profileName, int TP_Highscore, int SI_highscore)
    {
        // create highscore entry
        HighscoreEntry highscoreEntry = new()
        { profileName = profileName, TP_highscore = TP_Highscore, SI_highscore = SI_highscore };

        // load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // add entry to highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // save and update highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void CreateHighscoreEntryTransform(PlayerData data, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight) * transformList.Count;
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("Name Label").GetComponent<TextMeshProUGUI>().SetText(data.playerName);
        entryTransform.Find("TP Score Label").GetComponent<TextMeshProUGUI>().SetText($"{data.profile_TP_TotalScore}");
        entryTransform.Find("SI Score Label").GetComponent<TextMeshProUGUI>().SetText($"{data.profile_SI_TotalScore}");

        entryTransform.Find("Background").gameObject.SetActive(transformList.Count % 2 == 1); // Assuming even/odd row background logic

        transformList.Add(entryTransform);
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public string profileName;
        public int TP_highscore;
        public int SI_highscore;
    }
}