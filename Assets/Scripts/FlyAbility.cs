using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAbility : MonoBehaviour
{
    [Header("Fly Settings")]
    public float flyGravityScale = 0.4f;
    public float ascendSpeed = 3f;
    public float descendSpeed = -2f;
    public float airMoveSpeed = 4f;

    private Rigidbody2D rb;
    private Animator anim;
    private float originalGravity;
    private bool isFlying = false;

    private PlayerController controller;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        if (isFlying)
            HandleFlyControl();
    }

    public void EnableFly()
    {
        isFlying = true;
        rb.gravityScale = flyGravityScale;
        anim.SetBool("IsFlying", true);
    }

    public void DisableFly()
    {
        isFlying = false;
        rb.gravityScale = originalGravity;
        anim.SetBool("IsFlying", false);
    }

    private void HandleFlyControl()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * airMoveSpeed, rb.velocity.y);

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, ascendSpeed);
        }
        else if (rb.velocity.y > descendSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.MoveTowards(rb.velocity.y, descendSpeed, Time.deltaTime * 5f));
        }
    }

    public bool IsFlying()
    {
        return isFlying;
    }
}
