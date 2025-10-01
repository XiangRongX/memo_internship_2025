using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    public Sprite arrowLeft;
    public Sprite arrowRight;

    [Header("Arrow Settings")]
    public float lifetimeOnWall = 7f; // ͣ��ʱ��
    public float blinkDuration = 2f;  // ��˸ʱ��

    private bool stuck = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        col.isTrigger = false;
        rb.freezeRotation = true;
    }

    public void Launch(Vector2 direction, float speed, bool facingRight)
    {
        sr.sprite = facingRight ? arrowRight : arrowLeft;
        rb.velocity = direction * speed;
        rb.gravityScale = 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����������������ײ
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Physics2D.IgnoreCollision(collision.collider, col);
            return;
        }

        // ��������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        // ����ǽ����� -> ����
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            stuck = true;

            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.freezeRotation = true;

            // ��ӵ���ƽ̨���
            if (GetComponent<PlatformEffector2D>() == null)
                gameObject.AddComponent<PlatformEffector2D>();

            StartCoroutine(ArrowLifetime());
        }
    }

    private IEnumerator ArrowLifetime()
    {
        float timer = 0f;
        while (timer < lifetimeOnWall)
        {
            timer += Time.deltaTime;

            // ��˸
            if (timer > lifetimeOnWall - blinkDuration)
            {
                float t = (timer - (lifetimeOnWall - blinkDuration)) / blinkDuration;
                float blink = Mathf.Sin(t * t * 30f);
                sr.enabled = blink > 0;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
