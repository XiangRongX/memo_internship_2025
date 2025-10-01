using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;  // Z = -10

    private void LateUpdate()
    {
        if(player == null) return;

        // Ŀ��λ�ã����+ƫ��
        Vector3 desiredPosition = player.position + offset;

        // ƽ����ֵ
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
