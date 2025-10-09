using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;

    public AudioClip shootClip; 
    public float volume = 1f;   

    private bool facingRight = true;
    private Animator anim;

    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        float move = Input.GetAxisRaw("Horizontal");
        if(move > 0) facingRight = true;
        if(move < 0) facingRight = false;
    }

    private void Shoot()
    {
        anim.SetTrigger("Shoot");

        AudioSource.PlayClipAtPoint(shootClip, Camera.main.transform.position, volume);
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {

        yield return new WaitForSeconds(0.3f);

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        arrowScript.Launch(direction, arrowSpeed, facingRight, col);
    }
}
