using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapPlatform : MonoBehaviour
{
    [Header("Trap Platform Settings")]
    public float disappearDelay = 1f;
    public float reappearDelay = 3f;
    public bool isActive = true;

    private CompositeCollider2D col;
    private TilemapRenderer tr;
    private TilemapCollider2D tilemapCol;

    void Start()
    {
        col = GetComponent<CompositeCollider2D>();
        tilemapCol = GetComponent<TilemapCollider2D>();
        tr = GetComponent<TilemapRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // ��ȡ��ײ��
            ContactPoint2D[] contacts = collision.contacts;
            foreach (var contact in contacts)
            {
                // �����ײ��ķ��߳��ϣ�˵��վ��ƽ̨�ϣ�
                if (contact.normal.y < -0.5f)
                {
                    StartCoroutine(TrapSequence());
                    break;
                }
            }
        }
    }

    private IEnumerator TrapSequence()
    {
        yield return new WaitForSeconds(disappearDelay);

        SetPlatformActive(false);

        yield return new WaitForSeconds(reappearDelay);

        SetPlatformActive(true);
    }

    private void SetPlatformActive(bool active)
    {
        col.enabled = active;
        tilemapCol.enabled = active;
        tr.enabled = active;
        isActive = active;
    } 
}
