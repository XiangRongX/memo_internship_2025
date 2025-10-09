using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject winMenu;

    private int enemyCount = 0;

    private void Start()
    {
        // 统计敌人数量
        enemyCount = FindObjectsOfType<MonoBehaviour>().Count(x =>
            x is Slime || x is BlueMob || x is BlackMob);

        if (winMenu != null) winMenu.SetActive(true);
    }

    public void OnEnemyDied()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            Win();
        }
    }

    private void Win()
    {
        if (winMenu != null) winMenu.SetActive(true);

        Time.timeScale = 0f; // 暂停游戏
    }
}
