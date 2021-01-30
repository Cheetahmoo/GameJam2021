using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    Rigidbody2D rb;
    Vector2 playerInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        Move();
    }

    void Move()
    {
        //rb.AddForce(playerInput * Time.deltaTime);
        rb.velocity = playerInput * Time.deltaTime;
    }

    void GetInput()
    {
        //get player inputs
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
        //normalize and scale movement vector
        playerInput.Normalize();
        playerInput *= moveSpeed;
    }
}
