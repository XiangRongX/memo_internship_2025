using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public float invulnerableTime = 3f;  // Œﬁµ– ±º‰
    public float blinkInterval = 0.2f;   // …¡À∏º‰∏Ù

    private int currentHealth;
    private bool isInvulnerable = false;
    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enermy") || collision.CompareTag("Trap"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        float timer = 0f;

        while (timer < invulnerableTime)
        {
            sr.enabled = !sr.enabled; // …¡À∏
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        sr.enabled = true; 
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("ÕÊº“À¿Õˆ£°");
        
        // TODO: À¿Õˆ¬ﬂº≠
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
