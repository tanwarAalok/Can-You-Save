using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float jumpSpeed = 10;
    private CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() 
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * runSpeed, body.velocity.y);

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")) && Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }
}
