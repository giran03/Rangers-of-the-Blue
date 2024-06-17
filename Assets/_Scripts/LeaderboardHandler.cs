using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardHandler : MonoBehaviour
{
    Transform entryContainer;
    Transform entryTemplate;
    float templateHeight = 35f;
    List<Transform> highscoreEntryTransformList;
    List<PlayerData> playerDataList;

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



        // prints all of the store player data
        // foreach (PlayerData data in playerDataList)
        // {
        //     Debug.Log($"- Player Name: {data.playerName}");
        //     Debug.Log($"- TP Score: {data.profile_TP_TotalScore}");
        //     Debug.Log($"- SI Score: {data.profile_SI_TotalScore}");
        // }

        // get the highscores from json
        // string jsonString = PlayerPrefs.GetString("highscoreTable");
        // Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // TODO: SP Gamemode Sorting
        // sorted by Trash Picking Scores
        // for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        // {
        //     for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
        //     {
        //         if (highscores.highscoreEntryList[j].TP_highscore > highscores.highscoreEntryList[i].TP_highscore)
        //         {
        //             // swap
        //             (highscores.highscoreEntryList[j], highscores.highscoreEntryList[i]) = (highscores.highscoreEntryList[i], highscores.highscoreEntryList[j]);
        //         }
        //     }
        // }

        // highscoreEntryTransformList = new();
        // foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        // {
        //     CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        // }

        playerDataList = SaveSystem.GetPlayerData(); // Get data for all .fish files

        for (int i = 0; i < playerDataList.Count; i++)
        {
            for (int j = i + 1; j < playerDataList.Count; j++)
            {
                if (playerDataList[j].profile_SI_TotalScore > playerDataList[i].profile_SI_TotalScore)
                {
                    (playerDataList[j], playerDataList[i]) = (playerDataList[i], playerDataList[j]);    // swap
                }
            }
        }

        highscoreEntryTransformList = new();
        foreach (PlayerData data in playerDataList)
        {
            CreateHighscoreEntryTransform(data, entryContainer, highscoreEntryTransformList);
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

    private void CreateHighscoreEntryTransform(PlayerData data, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("Name Label").GetComponent<TextMeshProUGUI>().SetText(data.playerName);
        entryTransform.Find("TP Score Label").GetComponent<TextMeshProUGUI>().SetText($"{data.profile_TP_TotalScore}");
        entryTransform.Find("SI Score Label").GetComponent<TextMeshProUGUI>().SetText($"{data.profile_SI_TotalScore}");

        entryTransform.Find("Background").gameObject.SetActive(transformList.Count % 2 == 1); // Assuming even/odd row background logic

        transformList.Add(entryTransform);
    }

    // private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    // {
    //     Transform entryTransform = Instantiate(entryTemplate, container);
    //     RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
    //     entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight * transformList.Count);
    //     entryTransform.gameObject.SetActive(true);

    //     entryTransform.Find("Name Label").GetComponent<TextMeshProUGUI>().SetText(highscoreEntry.profileName);
    //     entryTransform.Find("TP Score Label").GetComponent<TextMeshProUGUI>().SetText($"{highscoreEntry.TP_highscore}");
    //     entryTransform.Find("SI Score Label").GetComponent<TextMeshProUGUI>().SetText($"{highscoreEntry.SI_highscore}");

    //     entryTransform.Find("Background").gameObject.SetActive(transformList.Count % 2 == 1);

    //     transformList.Add(entryTransform);
    // }

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