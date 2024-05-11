using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button easyButton;
    public Button difficultButton;
    [SerializeField] int score;
    [SerializeField] int highScore;
    public GameObject cylinderPrefab;
    public float scaleRate = 100;
    public float scaleTime;
    public float scaleDuration;
    public float powerUpCount = 0;
    public bool isGameOn;

    // Update is called once per frame
    void Update()
    {
        scaleDuration = scaleRate / scaleTime;
    }

    public void StartGame(int difficulty)
    {
        easyButton.gameObject.SetActive(false);
        difficultButton.gameObject.SetActive(false);
        isGameOn = true;
        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        score = 0;
        scaleTime = difficulty;
        UpdateScore(0);
        CylinderSpawner();
    }

    public void CylinderSpawner()
    {
        if (isGameOn)
        {
            Instantiate(cylinderPrefab, transform.position, cylinderPrefab.transform.rotation);
        }
    }

    public void GameOver()
    {
        isGameOn = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        SaveHighScore();
    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SaveHighScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            if (score > PlayerPrefs.GetInt("highScore"))
            {
                highScore = score;
                PlayerPrefs.SetInt("highScore", highScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("highScore", highScore);
                PlayerPrefs.Save();
            }
        }
    }
}
