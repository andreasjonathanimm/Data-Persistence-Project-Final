using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    private int bricksLeft = 0;
    private int m_Points;

    public Text ScoreText;
    public GameObject GameOverText;
    public GameManager gameManager;

    public Rigidbody Ball;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBricks();
        StartBall();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        bricksLeft = FindObjectsOfType<Brick>().Length;
        if (bricksLeft == 0)
        {
            LineCount++;
            if (LineCount > 8)
            {
                LineCount = 8;
            }
            SpawnBricks();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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

    public void StartBall()
    {
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }
    public void GameOver()
    {
        GameOverText.SetActive(true);
    }
}
