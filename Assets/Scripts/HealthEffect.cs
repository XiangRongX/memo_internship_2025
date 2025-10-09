using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEffect : MonoBehaviour
{
    [Header("Heal Settings")]
    public int healAmount = 1;          
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����� Player ��ǩ
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }

            // ������Ч
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            }

            // �����Լ�
            Destroy(gameObject);
        }
    }
}
