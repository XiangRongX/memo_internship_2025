using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedupAbility : MonoBehaviour
{
    private bool isSpeeduped = false;
    private float originalSpeed;
    private Coroutine boostCoroutine;

    private PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        if (controller != null)
            originalSpeed = controller.moveSpeed;
    }

    public void EnableSpeedup(float multiplier)
    {
        if (controller == null) return;

        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        boostCoroutine = StartCoroutine(SpeedupRoutine(multiplier));
    }

    private IEnumerator SpeedupRoutine(float multiplier)
    {
        isSpeeduped = true;
        controller.moveSpeed = originalSpeed * multiplier;
        yield break; // 由EffectManager控制持续时间
    }

    public void DisableSpeedup()
    {
        if (controller != null)
            controller.moveSpeed = originalSpeed;

        isSpeeduped = false;

        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
            boostCoroutine = null;
        }
    }

    public bool IsSpeeduped()
    {
        return isSpeeduped;
    }
}
