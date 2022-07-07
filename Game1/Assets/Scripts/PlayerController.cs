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
    private Animator anim;
    SpriteRenderer sprite;


    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * runSpeed, body.velocity.y);
        Run();
        Jump();
        FlipSprite();
        Attack();
        ChangeColor();
    }


    void ChangeColor()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            sprite.color = new Color(1, 1, 1, 0.6f);
        }
        else sprite.color = new Color(1,1,1,1);
    }

    public bool Attack()
    {
        bool hasHorizontalSpeed = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;

        if (Input.GetKeyDown(KeyCode.LeftControl) && !hasHorizontalSpeed)
        {
            anim.Play("attack");
            return true;
        }
        return false;
    }

    void Run()
    {
        // Run animation
        bool hasHorizontalSpeed = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;
        anim.SetBool("Run", hasHorizontalSpeed);
    }

    void Jump()
    {
        // Jumping
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")) && Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            anim.Play("Jump");
        }
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(body.velocity.x), 1f);
        }
    }
}
