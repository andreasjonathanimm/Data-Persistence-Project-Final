using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 1;
    private int BricksLeft = 0;
    public string newName;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text StartText;
    private Button enterButton;
    public GameObject GameOverText;
    public GameObject theButton;
    public GameManager gameManager;
    public AudioClip startClip;
    private AudioSource audioSource;
    private HighScoreTable highscoresTable;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnBricks();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        highscoresTable = GameObject.Find("HighScoreTable").GetComponent<HighScoreTable>();
        enterButton = theButton.GetComponent<Button>();
        audioSource = gameManager.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                audioSource.PlayOneShot(startClip);
                StartText.gameObject.SetActive(false);
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                enterButton.onClick.Invoke();
            }
        }
        BricksLeft = FindObjectsOfType<Brick>().Length;
        if (BricksLeft <= 0)
        {
            LineCount++;
            if (LineCount > 8)
            {
                LineCount = 8;
            }
            SpawnBricks();
        }
    }

    public void SpawnBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5, 5, 8, 8 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        UI_InputWindow.Show_Static("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ");
        GameOverText.SetActive(true);
    }

    public void OnClick()
    {
        highscoresTable.AddHighscoreEntry(m_Points, newName);
        if (gameManager != null) gameManager.RestartGame();
        else SceneManager.LoadScene(0);
    }
}
