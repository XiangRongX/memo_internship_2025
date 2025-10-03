using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinePlatform : MonoBehaviour
{
    [Header("Spine Platform Settings")]
    public int damage = 1;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 玩家要踩在上面
                if (contact.normal.y < -0.5f)
                {
                    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                    }
                    break;
                }
            }
        }
    }
}
