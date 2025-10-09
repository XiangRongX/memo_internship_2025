using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilverCoin : MonoBehaviour
{
    [Header("Coin Settings")]
    public float lifeTime = 10f;
    public float blinkTime = 3f;
    public AudioClip pickupSound;
    public float bounceForce = 5f; // 弹力

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool collected = false;
    private bool hasBounced = false; // 是否已经弹过一次

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 2f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        StartCoroutine(LifetimeRoutine());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collected) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            if (!hasBounced)
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);
                hasBounced = true;
            }
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifeTime - blinkTime);

        float timer = 0f;
        while (timer < blinkTime)
        {
            timer += Time.deltaTime;
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }

    public void Collect()
    {
        if (collected) return;
        collected = true;

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);

        Destroy(gameObject);
    }

}
