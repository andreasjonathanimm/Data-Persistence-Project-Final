using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private List<HighscoreEntry> highscoreEntryList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Saves a Highscore table if it hasn't been declared or saved first
        if (!PlayerPrefs.HasKey("highscoreTable"))
        {
            Highscores tempHighscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>() {

                new HighscoreEntry{ score = 5, name = "AAA" },
                new HighscoreEntry{ score = 10, name = "BBB" },
                new HighscoreEntry{ score = 15, name = "CCC" },
                new HighscoreEntry{ score = 20, name = "DDD" },
                new HighscoreEntry{ score = 25, name = "EEE" },
                new HighscoreEntry{ score = 30, name = "FFF" },
                new HighscoreEntry{ score = 35, name = "GGG" },
                new HighscoreEntry{ score = 40, name = "HHH" },
                new HighscoreEntry{ score = 45, name = "III" },
                new HighscoreEntry{ score = 50, name = "JJJ" },
            }
            };
            string json = JsonUtility.ToJson(tempHighscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }

        // Loads the Highscore table
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Bubble sorts the Highscore table
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        // Create a list of Highscore UI
        highscoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < 10; i++)
        {
            CreateHighScoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighScoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 25f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int score = highscoreEntry.score;
        entryTransform.Find("hScore Text").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("Name Text").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int score, string name)
    {
        // Create highscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}