using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpSpeed = 10;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() 
    {
        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        if(isGrounded() && Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }


}
