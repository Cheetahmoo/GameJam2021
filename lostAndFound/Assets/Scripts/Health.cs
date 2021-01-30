using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool hasInitialized = false;
    public bool damagedReturn;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasInitialized && health <= 0)
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
        if ((canHurtPlayer && isPlayer) || !isPlayer)
        {
            health -= d;
        }
        return damagedReturn;
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
