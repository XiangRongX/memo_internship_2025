using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 10f;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();
        
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        arrowScript.Launch(direction, arrowSpeed, facingRight);
    }
}
