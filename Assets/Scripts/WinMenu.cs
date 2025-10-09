using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public GameObject winPanel;
    public TextMeshProUGUI levelScoreText;
    public TextMeshProUGUI totalScoreText;
    public Button continueButton;
    public Button endButton;

    public string NextLevelName;

    private int enemyCount = 0;
    private PlayerScore playerScore;
    private ScoreManager scoreManager;

    void Start()
    {
        winPanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueClicked);
        endButton.onClick.AddListener(OnEndClicked);

        playerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScore>();
        scoreManager = FindObjectOfType<ScoreManager>();

        // 统计敌人数量
        enemyCount = FindObjectsOfType<MonoBehaviour>().Count(x =>
            x is Slime || x is BlueMob || x is BlackMob);
    }

    public void OnEnemyDied()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            ShowWinUI(playerScore.GetLevelScore(), scoreManager.GetTotalScore());
        }
    }

    public void ShowWinUI(int levelScore, int totalScore)
    {
        levelScoreText.text = levelScore.ToString();
        totalScoreText.text = totalScore.ToString();
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnContinueClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(NextLevelName);
    }

    public void OnEndClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
