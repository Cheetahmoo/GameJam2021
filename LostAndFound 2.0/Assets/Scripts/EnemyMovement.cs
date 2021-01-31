using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float viewRadius;
    public GameObject player;
    public float rayOffset;
    public bool distCheck;
    public bool sightCheck;
    public bool playerActive;
    Rigidbody2D rb;
    public float moveSpeed;
    public float minDist;
    Gun gun;
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GetPlayer();
        distCheck = false;
        sightCheck = false;
        playerActive = false;
        gun = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            GetPlayer();
        }

        if (player && !playerActive)
        {
            distCheck = CheckDist(player.transform.position, viewRadius);
            sightCheck = CheckSight(player.transform.position);
        }

        if(distCheck && sightCheck)
        {
            playerActive = true;
        }


    }

    private void FixedUpdate()
    {
        if (playerActive && player)
        {
            Move();
            Shoot();
        }
    }

    bool GetPlayer()
    {
        GameObject []players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length != 0)
        {
            player = players[0];
            return true;
        }
        return false;
    }

    bool CheckDist(Vector3 target, float targetDist, bool checkDir = false)
    {
        float dist = Vector3.Distance(target, transform.position);
        if (dist <= targetDist && checkDir == false)
        {
            return true;
        }
        if(dist >= targetDist && checkDir == true)
        {
            return true;
        }
        return false;
    }

    bool CheckSight(Vector3 target)
    {
        Vector3 aimDir = target - transform.position;
        aimDir.Normalize();
        aimDir *= rayOffset;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position + aimDir, target - (transform.position + aimDir));
        //Debug.DrawLine(transform.position + aimDir, target, Color.green);
        Debug.Log(hit.collider);
        if (!hit.collider)
        {
            return false;
        }
        if (hit.collider.gameObject.tag == "Player")
        {
            return true;
        }

        return false;
    }

    void Move()
    {
        Vector3 aimDir = player.transform.position - transform.position;
        aimDir = aimDir.normalized * moveSpeed;
        if(CheckDist(player.transform.position, minDist, true)){
            rb.velocity = aimDir;
        }
        else
        {
            rb.velocity /= 5;
        }
        gun.transform.rotation = TurnToFace(player.transform.position);

        if (sr)
        {
            if (player.transform.position.x > transform.position.x)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }

    void Shoot()
    {
        if (gun)
        {
            gun.MechFire();
        }

    }

    Quaternion TurnToFace(Vector3 target, float offset = 0)
    {
        Vector3 aimDir = target - transform.position;
        float theta = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(theta + offset, Vector3.forward);
        return targetRotation;
    }

    public void PlayerActivate()
    {
        playerActive = true;
    }
}