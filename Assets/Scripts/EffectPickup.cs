using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectPickup : MonoBehaviour
{
    [Header("Effect Settings")]
    public EffectType effectType;     // ����/����/�޵�
    public float duration = 5f;       // ����ʱ��
    public float value = 1.5f;        // ���ٱ���
    public AudioClip pickupSound;     // ʰȡ��Ч

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EffectManager effectManager = collision.GetComponent<EffectManager>();

            if (effectManager != null)
            {
                effectManager.ApplyEffect(effectType, duration, value);
            }

            if (pickupSound)
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);

            Destroy(gameObject);
        }
    }
}

