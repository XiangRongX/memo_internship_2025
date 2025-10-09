using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectPickup : MonoBehaviour
{
    [Header("Effect Settings")]
    public EffectType effectType;     // 加速/飞行/无敌
    public float duration = 5f;       // 持续时间
    public float value = 1.5f;        // 加速倍率
    public AudioClip pickupSound;     // 拾取音效

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

