using System.Collections;
using UnityEngine;

public enum EffectType
{
    None,
    Speedup,
    Fly,
    Invulnerable
}

public class EffectManager : MonoBehaviour
{
    public AudioClip effectClip;
    public float volume = 2f;

    private EffectType currentEffect = EffectType.None;
    private Coroutine currentEffectRoutine;

    private SpeedupAbility speedup;
    private FlyAbility fly;
    private InvulnerableAbility invulnerable;

    public EffectUI effectUI;   

    void Start()
    {
        speedup = GetComponent<SpeedupAbility>();
        fly = GetComponent<FlyAbility>();
        invulnerable = GetComponent<InvulnerableAbility>();
    }

    public void ApplyEffect(EffectType type, float duration, float value = 1f)
    {
        AudioSource.PlayClipAtPoint(effectClip, Camera.main.transform.position, volume);

        // 取消现有效果
        if (currentEffectRoutine != null)
        {
            StopCoroutine(currentEffectRoutine);
            EndCurrentEffect();
        }

        // 启动新效果
        currentEffect = type;
        UpdateEffectUI(); 
        currentEffectRoutine = StartCoroutine(EffectRoutine(type, duration, value));
    }

    private IEnumerator EffectRoutine(EffectType type, float duration, float value)
    {
        switch (type)
        {
            case EffectType.Speedup:
                if (speedup) speedup.EnableSpeedup(value);
                break;
            case EffectType.Fly:
                if (fly) fly.EnableFly();
                break;
            case EffectType.Invulnerable:
                if (invulnerable) invulnerable.EnableInvincible();
                break;
        }

        yield return new WaitForSeconds(duration);
        EndCurrentEffect();
    }

    private void EndCurrentEffect()
    {
        switch (currentEffect)
        {
            case EffectType.Speedup:
                if (speedup) speedup.DisableSpeedup();
                break;
            case EffectType.Fly:
                if (fly) fly.DisableFly();
                break;
            case EffectType.Invulnerable:
                if (invulnerable) invulnerable.DisableInvincible();
                break;
        }

        currentEffect = EffectType.None;
        UpdateEffectUI(); 
        currentEffectRoutine = null;
    }

    private void UpdateEffectUI()
    {
        if (effectUI)
            effectUI.UpdateEffectIcon(currentEffect);
    }

    public EffectType GetCurrentEffect()
    {
        return currentEffect;
    }
}
