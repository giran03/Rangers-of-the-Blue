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
                // sort by profile names
                for (int i = 0; i < playerDataList.Count; i++)
                {
                    for (int j = i + 1; j < playerDataList.Count; j++)
                    {
                        if (string.Compare(playerDataList[j].playerName, playerDataList[i].playerName) < 0)
                        {
                            (playerDataList[j], playerDataList[i]) = (playerDataList[i], playerDataList[j]);    // swap
                        }
                    }
                }

        /*
        // sort by TP SCORES
        for (int i = 0; i < playerDataList.Count; i++)
        {
            for (int j = i + 1; j < playerDataList.Count; j++)
            {
                if (playerDataList[j].profile_TP_TotalScore > playerDataList[i].profile_TP_TotalScore)
                {
                    (playerDataList[j], playerDataList[i]) = (playerDataList[i], playerDataList[j]);    // swap
                }
            }
        }
        */
        highscoreEntryTransformList = new();
        foreach (PlayerData data in playerDataList)
            CreateHighscoreEntryTransform(data, entryContainer, highscoreEntryTransformList);
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
}