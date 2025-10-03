using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWarpManager : MonoBehaviour
{
    [Header("Map Bound")]
    public float leftBound = -20f;
    public float rightBound = 20f;
    public float bottomBound = -10f;
    public float topBound = 10f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 pos = player.position;

        // 左右循环
        if (pos.x < leftBound) pos.x = rightBound;
        else if (pos.x > rightBound) pos.x = leftBound;

        // 上下循环
        if (pos.y < bottomBound) pos.y = topBound;
        else if (pos.y > topBound) pos.y = bottomBound;

        player.position = pos;
    }

    // 可视化地图边界
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((leftBound + rightBound) / 2, (bottomBound + topBound) / 2, 0);
        Vector3 size = new Vector3(rightBound - leftBound, topBound - bottomBound, 0);
        Gizmos.DrawWireCube(center, size);
    }
}
