using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isPaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;   // ֹͣ��Ϸʱ��
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;   // �ָ�ʱ��
        isPaused = false;
    }

    public void EndGame()
    {
        Time.timeScale = 1f;   // ȷ��ʱ��ָ�
        SceneManager.LoadScene("MainMenu"); 
    }
}
