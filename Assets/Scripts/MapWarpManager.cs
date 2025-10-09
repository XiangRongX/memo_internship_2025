using UnityEngine;

public class MapWarpManager : MonoBehaviour
{
    [Header("Map Bound")]
    public float leftBound = -20f;
    public float rightBound = 20f;
    public float bottomBound = -10f;
    public float topBound = 10f;

    [Header("Tags to Wrap")]
    public string playerTag = "Player";
    public string[] enemyTags = { "Slime", "BlueMob", "BlackMob" };

    private Transform player;
    private Rigidbody2D playerRb;

    void Start()
    {
        // 找到玩家
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate()
    {
        // 玩家循环
        if (player != null)
            WrapObject(player, playerRb);

        // 敌人循环
        foreach (string tag in enemyTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue;

                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                WrapObject(enemy.transform, rb);
            }
        }
    }

    private void WrapObject(Transform obj, Rigidbody2D rb)
    {
        Vector3 pos = obj.position;
        bool warped = false;

        // 左右循环
        if (pos.x < leftBound)
        {
            pos.x = rightBound;
            warped = true;
        }
        else if (pos.x > rightBound)
        {
            pos.x = leftBound;
            warped = true;
        }

        // 上下循环
        if (pos.y < bottomBound)
        {
            pos.y = topBound;
            warped = true;
            if (rb != null)
                rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        else if (pos.y > topBound)
        {
            pos.y = bottomBound;
            warped = true;
            if (rb != null)
                rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        if (warped)
            obj.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((leftBound + rightBound) / 2, (bottomBound + topBound) / 2, 0);
        Vector3 size = new Vector3(rightBound - leftBound, topBound - bottomBound, 0);
        Gizmos.DrawWireCube(center, size);
    }
}
