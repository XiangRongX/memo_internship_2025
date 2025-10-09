using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private Animator anim;

    public AudioClip jumpClip;
    public float volume = 2f;

    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //AD  A=-1 D=1
        float move = Input.GetAxisRaw("Horizontal");
        if (move > 0)
        {
            facingRight = true;
        }
        else if (move < 0)
        {
            facingRight = false;
        }
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        //W
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioSource.PlayClipAtPoint(jumpClip, Camera.main.transform.position, volume);
        }

        anim.SetFloat("Speed", Mathf.Abs(move * moveSpeed));
        anim.SetFloat("FacingRight", facingRight ? 1 : 0);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("VelocityY", rb.velocity.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Arrow") || collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Spine"))
        {
            bool grounded = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 只要有一个点在脚下，就算落地
                if (contact.normal.y > 0.5f)
                {
                    grounded = true;
                    break;
                }
            }
            isGrounded = grounded;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Arrow"))
        {
            // 离开这个物体后，可能还在另一个物体上，所以要检测当前所有接触
            isGrounded = false;
        }
    }
}
