using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    Transform entryContainer;
    Transform entryTemplate;
    float templateHeight = 35f;
    List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        // highscoreEntryList = new List<HighscoreEntry>()
        // {
        //     new() {profileName = "aaa", SI_highscore = 111, TP_highscore = 72},
        //     new() {profileName = "bbb", SI_highscore = 666, TP_highscore = 222},
        //     new() {profileName = "ccc", SI_highscore = 67, TP_highscore = 231},
        //     new() {profileName = "ddd", SI_highscore = 444, TP_highscore = 661},
        // };

        // get the highscores from json
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // TODO: SP Gamemode Sorting
        // sorted by Trash Picking Scores
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].TP_highscore > highscores.highscoreEntryList[i].TP_highscore)
                {
                    // swap
                    HighscoreEntry temp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = temp;
                }
            }
        }

        highscoreEntryTransformList = new();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
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

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("Name Label").GetComponent<TextMeshProUGUI>().SetText(highscoreEntry.profileName);
        entryTransform.Find("TP Score Label").GetComponent<TextMeshProUGUI>().SetText($"{highscoreEntry.TP_highscore}");
        entryTransform.Find("SI Score Label").GetComponent<TextMeshProUGUI>().SetText($"{highscoreEntry.SI_highscore}");

        entryTransform.Find("Background").gameObject.SetActive(transformList.Count % 2 == 1);

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