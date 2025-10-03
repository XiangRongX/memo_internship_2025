using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;  // Z = -10

    private MapWarpManager map;

    private float leftBound;
    private float rightBound;
    private float bottomBound ;
    private float topBound;

    private void Start()
    {
        map = FindFirstObjectByType<MapWarpManager>();
        leftBound = map.leftBound;
        rightBound = map.rightBound;
        bottomBound = map.bottomBound;
        topBound = map.topBound;
    }

    private void LateUpdate()
    {
        if(player == null) return;

        // 目标位置：玩家+偏移
        Vector3 desiredPosition = player.position + offset;
        desiredPosition.z = offset.z;

        // 计算相机半宽高
        float camHalfHeight = Camera.main.orthographicSize;
        float camHalfWidth = camHalfHeight * Camera.main.aspect;

        // 限制相机位置在地图边界内
        float minX = leftBound + camHalfWidth;
        float maxX = rightBound - camHalfWidth;
        float minY = bottomBound + camHalfHeight;
        float maxY = topBound - camHalfHeight;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // 平滑移动
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
