using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public float invulnerableTime = 3f;  // �޵�ʱ��
    public float blinkInterval = 0.2f;   // ��˸���

    public AudioClip hurtClip;
    public AudioClip dieClip;
    public float volume = 1f;

    public HealthUI healthUI;

    private int currentHealth;
    private bool isInvulnerable = false;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private InvulnerableAbility invulnerableAbility;
    private Animator anim;


    void Start()
    {
        currentHealth = maxHealth;
        healthUI.UpdateHealth(currentHealth);
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        invulnerableAbility = GetComponent<InvulnerableAbility>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isInvulnerable)
        {
            if (invulnerableAbility != null && invulnerableAbility.IsInvulnerable())
            {
                isInvulnerable = true;
            }
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // ���ƽ̨��Ѫ�߼���ƽ̨����
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void TakeDamage(int damage, Transform source = null)
    {
        if (isInvulnerable) return;

        AudioSource.PlayClipAtPoint(hurtClip, Camera.main.transform.position, volume);
        currentHealth -= damage;
        healthUI.UpdateHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        Knockback(source);

        StartCoroutine(InvulnerabilityCoroutine());
    }

    private void Knockback(Transform source)
    {
        if (rb == null) return;

        Vector2 force = Vector2.up * 10f; 

        if (source != null)
        {
            // ������Դ����������ҵ���
            float dir = transform.position.x < source.position.x ? -1f : 1f;
            force += Vector2.right * dir * 3f;
        }

        rb.velocity = Vector2.zero; // ���ԭ�����ٶȣ��������
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthUI.UpdateHealth(currentHealth);
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        float timer = 0f;

        while (timer < invulnerableTime)
        {
            sr.enabled = !sr.enabled; // ��˸
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        sr.enabled = true; 
        isInvulnerable = false;
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(dieClip, Camera.main.transform.position, volume);

        anim.SetBool("IsDead", true);

        StartCoroutine(RestartAfterDelay(1.0f));  // 1���ӳ�
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���ñ���
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
