using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverCoinTrigger : MonoBehaviour
{
    private SilverCoin coin;

    private void Awake()
    {
        coin = GetComponentInParent<SilverCoin>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            coin.Collect();
        }
    }
}
