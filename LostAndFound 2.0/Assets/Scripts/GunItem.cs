using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Gun gun;
    public Sprite gunImage;
    public bool touchingPlayer;
    void Start()
    {
        touchingPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("A child");
            GunManager gm = collision.gameObject.GetComponentInChildren<GunManager>();
            if (gm)
            {
                gm.gi = this;
                touchingPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GunManager gm = collision.gameObject.GetComponentInChildren<GunManager>();
            if (gm)
            {
                gm.gi = null;
                touchingPlayer = false;
            }
        }
    }
}
