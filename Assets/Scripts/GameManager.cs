using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq.Expressions;
using UnityEditor;


public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate = 1.0f;
    public bool isGameActive;
    public TextMeshProUGUI gameOverText, scoreText, TitleText, HighScore, newHighScore, livesText;
    public Button restartButton, homeButton, pauseButton, calmButton, furyButton, chaosButton;
    int score, highScore, lives = 5;
    public GameObject PauseMenuPanel, DifficultySelectionPanel;
    public AudioCheck isCheck;
    private int difficulty;
    private string currentHighScoreKey;
    public Blade blade;
    public void Start()
    {
        difficulty = PlayerPrefs.GetInt("SelectedDifficulty", 1);
        SetHighScoreKey();
        LoadHighScore();
        UpdateLivesUI();

        blade.gameObject.SetActive(false);

        HighScore.text = "High Score - " + highScore;
        HighScore.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        newHighScore.gameObject.SetActive(false);

        calmButton.onClick.AddListener(() => SelectDifficulty(1));
        furyButton.onClick.AddListener(() => SelectDifficulty(2));
        chaosButton.onClick.AddListener(() => SelectDifficulty(3));

        UpdateDifficultyButtonColors();

    }
    public void StartGame()
    {
        blade.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);

        isGameActive = true;
        SetSpawnRate();
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        TitleText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        DifficultySelectionPanel.gameObject.SetActive(false);
    }

    /*spawning targets*/
    IEnumerator SpawnTarget()
    {
        while (isGameActive == true)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);

            Vector3 spawnPos = new(Random.Range(-13f, 21f), -11f, 0f);

            Instantiate(targets[index], spawnPos, targets[index].transform.rotation);

        }
    }

    /*utility functions*/
    private void SelectDifficulty(int level)
    {
        difficulty = level;
        PlayerPrefs.SetInt("SelectedDifficulty", difficulty);
        PlayerPrefs.Save();

        SetHighScoreKey();
        LoadHighScore();
        HighScore.text = "High Score - " + highScore;

        UpdateDifficultyButtonColors();
    }
    private void SetHighScoreKey()
    {
        switch (difficulty)
        {
            case 1:
                currentHighScoreKey = "HighScore_Calm";
                break;
            case 2:
                currentHighScoreKey = "HighScore_Fury";
                break;
            case 3:
                currentHighScoreKey = "HighScore_Chaos";
                break;
            default:
                currentHighScoreKey = "HighScore_Calm";
                break;
        }
    }
    private void SetSpawnRate()
    {
        switch (difficulty)
        {
            case 2:
                spawnRate = 0.75f;
                break;
            case 3:
                spawnRate = 0.5f;
                break;
            case 1:
            default:
                spawnRate = 1.0f;
                break;

        }
    }
    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(currentHighScoreKey, 0);
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score - " + score;

        if (score > PlayerPrefs.GetInt(currentHighScoreKey, 0))
        {
            PlayerPrefs.SetInt(currentHighScoreKey, score);
            PlayerPrefs.Save();
            HighScore.text = "High Score - " + score.ToString();
        }
    }
    private void UpdateDifficultyButtonColors()
    {
        Color normalColor = new Color(0.6235294f, 0.4509804f, 0.2392157f, 1f);
        Color selectedColor = new Color(0.8431373f, 0.5490196f, 0.3058824f, 1f);

        calmButton.GetComponent<Image>().color = (difficulty == 1) ? selectedColor : normalColor;
        furyButton.GetComponent<Image>().color = (difficulty == 2) ? selectedColor : normalColor;
        chaosButton.GetComponent<Image>().color = (difficulty == 3) ? selectedColor : normalColor;
    }
    public void LoseLife()
    {
        if (isGameActive == false)
            return;

        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }
    public void UpdateLivesUI()
    {
        livesText.text = "Lives - " + lives;

    }

    /*Button interaction for home, pause, resume, restart, gameover. */
    public void GameOver()
    {
        blade.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        scoreText.gameObject.SetActive(false);
        blade.bladeColl.enabled = false;
        Time.timeScale = 1f;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(currentHighScoreKey, highScore);
            PlayerPrefs.Save();

            newHighScore.text = "New High Score - " + highScore;
            newHighScore.gameObject.SetActive(true);
            HighScore.gameObject.SetActive(false);
        }
        else
        {
            HighScore.text = "High Score - " + highScore;
            HighScore.gameObject.SetActive(true);
        }

        livesText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }
    public void RestartGame()
    {
        restartButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        newHighScore.gameObject.SetActive(false);
        isGameActive = true;
        score = 0;
        lives = 5;
        blade.gameObject.SetActive(true);

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        UpdateLivesUI();
        livesText.gameObject.SetActive(true);

        scoreText.gameObject.SetActive(true);
        HighScore.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
    }
    public void Home()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        SetHighScoreKey();
        LoadHighScore();

        HighScore.text = "High Score - " + highScore;
        DifficultySelectionPanel.gameObject.SetActive(true);
    }
    public void Pause()
    {
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        PauseMenuPanel.SetActive(false);
        RestartGame();
    }
}
