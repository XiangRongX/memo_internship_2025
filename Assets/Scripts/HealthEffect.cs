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
        // 必须带 Player 标签
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }

            // 播放音效
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            }

            // 销毁自己
            Destroy(gameObject);
        }
    }
}
