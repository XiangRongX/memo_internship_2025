using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectUI : MonoBehaviour
{
    public Image effectImage;

    public Sprite invulnerable;
    public Sprite speedup;
    public Sprite fly;

    private void Start()
    {
        effectImage.enabled = false;
    }

    public void UpdateEffectIcon(EffectType type)
    {
        switch (type)
        {
            case EffectType.Speedup:
                effectImage.sprite = speedup;
                effectImage.enabled = true;
                break;
            case EffectType.Fly:
                effectImage.sprite = fly;
                effectImage.enabled = true;
                break;
            case EffectType.Invulnerable:
                effectImage.sprite = invulnerable;
                effectImage.enabled = true;
                break;
            default:
                effectImage.sprite = null;
                effectImage.enabled = false;
                break;
        }
    }
}
