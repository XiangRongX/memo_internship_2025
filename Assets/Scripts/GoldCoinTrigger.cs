using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoinTrigger : MonoBehaviour
{
    private GoldCoin coin;

    private void Awake()
    {
        coin = GetComponentInParent<GoldCoin>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            coin.Collect();
        }
    }
}
