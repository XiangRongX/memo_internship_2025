using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScore : MonoBehaviour
{
    private int levelScore = 0;
    public ScoreUI scoreUI;

    void Start()
    {
        levelScore = 0;
        scoreUI.ResetScore();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int add = 0;

        if (collision.CompareTag("SilverCoin")) add = 50;
        else if (collision.CompareTag("GoldCoin")) add = 100;

        if (add > 0)
        {
            levelScore += add;
            scoreUI.AddScore(add);
            ScoreManager.Instance.AddScore(add); // 全局同步
            Destroy(collision.gameObject);
        }
    }

    public int GetLevelScore() => levelScore;
}
