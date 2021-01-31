using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool hasInitialized = false;
    public bool damagedReturn;
    public bool isPlayer;
    public EnemySpawner es;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasInitialized && health <= 0)
        {
            Kill();
        }
    }

    public void SetHealth(int h)
    {
        health = h;
        hasInitialized = true;
    }

    public bool TakeDamage(int d, bool canHurtPlayer = true)
    {
        if (canHurtPlayer == isPlayer)
        {
            health -= d;
            if (!isPlayer)
            {
                EnemyMovement em = GetComponent<EnemyMovement>();
                if (em)
                {
                    em.PlayerActivate();
                }

            }
            return damagedReturn;
        }
        return !damagedReturn;
    }

    public void Kill()
    {
        if (isPlayer)
        {

        }
        else
        {
            es.Kill(this.gameObject);
        }
        //Destroy(this.gameObject);
    }
}