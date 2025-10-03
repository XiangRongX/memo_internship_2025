using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Arrow : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D col;

    public Sprite arrowLeft;
    public Sprite arrowRight;

    [Header("Arrow Settings")]
    public float lifetimeOnWall = 7f; // 停留时间
    public float blinkDuration = 2f;  // 闪烁时间

    private Collider2D playerCol;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        col.isTrigger = false;
        rb.freezeRotation = true;
    }

    public void Launch(Vector2 direction, float speed, bool facingRight, Collider2D playerCollider)
    {
        sr.sprite = facingRight ? arrowRight : arrowLeft;
        rb.velocity = direction * speed;
        rb.gravityScale = 0.5f;

        playerCol = playerCollider;
        // 飞行时忽略和玩家的碰撞
        Physics2D.IgnoreCollision(playerCol, col, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 忽略与其他箭的碰撞
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Physics2D.IgnoreCollision(collision.collider, col);
            return;
        }

        // 碰到敌人
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        // 碰到墙或地面 -> 插入
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.freezeRotation = true;

            // 恢复和玩家的碰撞
            if (playerCol != null)
            {
                Physics2D.IgnoreCollision(playerCol, col, false);
            }

            // 添加单向平台组件
            if (GetComponent<PlatformEffector2D>() == null)
            {
                gameObject.AddComponent<PlatformEffector2D>();
            }  

            StartCoroutine(ArrowLifetime());
        }
    }

    private IEnumerator ArrowLifetime()
    {
        float timer = 0f;
        while (timer < lifetimeOnWall)
        {
            timer += Time.deltaTime;

            // 闪烁
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
