using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulnerableAbility : MonoBehaviour
{
    private bool isInvulnerable = false;
    private SpriteRenderer sr;
    private Coroutine blinkCoroutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void EnableInvincible()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        isInvulnerable = true;
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    public void DisableInvincible()
    {
        isInvulnerable = false;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (sr != null)
            sr.enabled = true;
    }

    private IEnumerator BlinkRoutine()
    {
        float blinkInterval = 0.1f;
        bool visible = true;

        while (isInvulnerable)
        {
            visible = !visible;
            if (sr != null)
                sr.enabled = visible;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
