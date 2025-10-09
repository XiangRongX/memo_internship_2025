using UnityEngine;
using System.Collections;

public class BlackMob : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float checkDistance = 0.2f;
    public LayerMask obstacleLayer;
    public LayerMask groundLayer;
    public float jumpInterval = 2f;

    public Transform wallCheck;   
    public Transform groundCheck; 

    [Header("Death Settings")]
    public float deathBounceForce = 5f;
    public float deathDelay = 1.2f;

    public GameObject coinGoldPrefab;
    public GameObject coinSilverPrefab;
    public float goldDropChance = 0.3f;
    public AudioClip spawnClip;
    public float volume = 1f;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    private bool facingRight = false; 
    private bool isDead = false;
    private float jumpTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 1;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isDead) return;

        
        bool hitWall = false;
        if (wallCheck)
        {
            hitWall = Physics2D.BoxCast(
                wallCheck.position,
                new Vector2(0.2f, 1.1f),
                0f,
                Vector2.right * (facingRight ? 1 : -1),
                checkDistance,
                groundLayer
            );
        }


        bool groundAhead = true;
        if (IsGrounded() && groundCheck)
        {
            groundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);
        }

       
        if (hitWall || !groundAhead)
        {
            facingRight = !facingRight;
            UpdateSpriteFlip();
        }

     
        rb.velocity = new Vector2((facingRight ? 1 : -1) * moveSpeed, rb.velocity.y);

        
        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpInterval && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimer = 0f;
        }

       
        if (anim)
        {
            anim.SetBool("IsJumping", !IsGrounded());
            anim.SetBool("IsDead", isDead);
            anim.SetBool("FacingRight", facingRight);
        }
    }

    // 检测是否接触地面
    private bool IsGrounded()
    {
        if (groundCheck)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
            return hit.collider != null;
        }
        return false;
    }

    // 翻转 Sprite
    private void UpdateSpriteFlip()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    // 死亡
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        col.enabled = false;
        rb.velocity = new Vector2(rb.velocity.x, deathBounceForce);

        if (anim)
        {
            anim.SetBool("IsDead", true);
            anim.SetBool("FacingRight", facingRight); // 死亡动画左右翻转
        }

        DropCoin();

        StartCoroutine(DeathRoutine());
    }

    private void DropCoin()
    {
        AudioSource.PlayClipAtPoint(spawnClip, Camera.main.transform.position, volume);

        GameObject prefabToSpawn = UnityEngine.Random.value < goldDropChance ? coinGoldPrefab : coinSilverPrefab;

        if (prefabToSpawn != null)
        {

            Vector3 dropPos = transform.position + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0.3f, 0f);
            Instantiate(prefabToSpawn, dropPos, Quaternion.identity);
        }
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        rb.gravityScale = 1; // 开启下落
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

        // 通知关卡管理器
        GameObject winMenuObj = GameObject.Find("WinMenu");
        Transform managerTransform = winMenuObj.transform.Find("Manager");
        if (managerTransform != null)
        {
            WinMenu winMenuScript = managerTransform.GetComponent<WinMenu>();
            if (winMenuScript != null)
            {
                winMenuScript.OnEnemyDied();
            }
        }
    }

    // 撞击箭矢死亡
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            if (arrow != null && arrow.isFlying) Die();
        }
    }

    // 可视化检测点
    private void OnDrawGizmosSelected()
    {
        if (wallCheck)
        {
            Gizmos.color = Color.red;
            Vector3 dir = facingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawWireCube(wallCheck.position + dir * checkDistance, new Vector3(0.2f, 1.1f, 0.1f));
        }
        if (groundCheck)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * checkDistance);
        }
    }
}
