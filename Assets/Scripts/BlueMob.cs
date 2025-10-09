using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMob : MonoBehaviour
{
    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public Transform wallCheck;          // ǽ����
    public float checkDistance = 0.2f;
    public LayerMask groundLayer;

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
    private bool facingRight = true;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        rb.gravityScale = 0; // ����
    }

    private void Update()
    {
        if (isDead) return;

        // ����WallCheckλ��
        UpdateWallCheckPosition();

        // ����Ƿ���Ҫ��ͷ
        bool hitWall = Physics2D.BoxCast(
            wallCheck.position,
            new Vector2(0.2f, 1.8f),
            0f,
            Vector2.right * (facingRight ? 1 : -1),
            checkDistance,
            groundLayer
        );

        if (hitWall)
        {
            TurnAround();
        }

        // �ƶ�
        float moveDir = facingRight ? 1f : -1f;
        rb.velocity = new Vector2(moveDir * moveSpeed, 0);

        // ����
        if (anim) anim.SetBool("FacingRight", facingRight);

        Debug.Log("WallCheck: " + hitWall);
    }

    private void UpdateWallCheckPosition()
    {
        if (wallCheck == null) return;

        Vector3 pos = transform.position;
        pos.x += facingRight ? 0.85f : -0.85f;
        wallCheck.position = pos;
    }

    private void TurnAround()
    {
        facingRight = !facingRight;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        col.enabled = false;  // �ر���ײ
        rb.gravityScale = 1;
        rb.velocity = new Vector2(rb.velocity.x, deathBounceForce); // ����
        anim.SetBool("IsDead", true);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            if (arrow != null && arrow.isFlying)
            {
                Die();
            }
            else
            {
                Physics2D.IgnoreCollision(collision.collider, col, true);
            }
        }
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        rb.gravityScale = 1; // ��������
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

        // ֪ͨ�ؿ�������
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

    private void OnDrawGizmosSelected()
    {
        if (wallCheck)
        {
            Gizmos.color = Color.red;
            Vector3 dir = facingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawWireCube(wallCheck.position + dir * checkDistance, new Vector3(0.2f, 1.8f, 0.1f));
        }
    }
}

