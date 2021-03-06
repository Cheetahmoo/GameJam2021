﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public int speed;
    Rigidbody2D rb;
    public int damage;
    public bool canHurtPlayer;
    public float maxLifetime;
    float timeAlive;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(maxLifetime == 0)
        {
            maxLifetime = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if(timeAlive >= maxLifetime)
        {
            Destroy(this.gameObject);
        }
        if (rb)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("Bullet has null Rigid Body!");
        }
    }

    public void SetProperties(int speed_ = 0, int damage_ = 0, bool canHurtPlayer_ = false)
    {
        speed = speed_;
        damage = damage_;
        canHurtPlayer = canHurtPlayer_;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health h = collision.GetComponent<Health>();
        if (h)
        {
            bool result = h.TakeDamage(damage, canHurtPlayer);
            if (result)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
