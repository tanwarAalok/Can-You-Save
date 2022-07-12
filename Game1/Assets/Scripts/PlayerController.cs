using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    [Header("Speed")]
    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpSpeed = 10;
    private Animator anim;
    SpriteRenderer sprite;

    [Header("Health Bar")]
    [SerializeField] HealthBar healthBar;
    public static int currHealth = 100;
    static int levelHealth = 100;
    [SerializeField] int maxHealth = 100;

    [Header("Game Manager")]
    GameManager gameManager;
    [SerializeField] bool gameOver = false;
    [SerializeField] GameObject door;
    float distanceFromDoor = 0;

    [Header("Attack")]
    [SerializeField] float waitBetweenAttack;
    [SerializeField] float startWaitBetweenAttack;
    private void Awake() 
    {
        gameManager = FindObjectOfType<GameManager>();
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currHealth = levelHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currHealth);
    }

    private void Update() 
    {
        if(!gameOver)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalInput * runSpeed, body.velocity.y);
            Run();
            Jump();
            FlipSprite();
            AttackEnemy();
            ChangeColor();
            healthBar.SetHealth(currHealth);
            OpenDoor();
            if(feetCollider.IsTouchingLayers(LayerMask.GetMask("Zombie")) && !feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")))
            {
                float moveALitte = 0.02f * transform.localScale.x;
                transform.position = new Vector2(transform.position.x + moveALitte, transform.position.y);
            }
        }

        if(currHealth <= 0 || feetCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle"))) {
            anim.Play("dead");
            body.velocity = new Vector2(0, body.velocity.y);
            gameOver = true;
            gameManager.PlayerDeadState(gameOver);
        }
    }
    
    void OpenDoor()
    {
        distanceFromDoor = Mathf.Abs(door.transform.position.x - transform.position.x);
        if(distanceFromDoor < 1f && Input.GetKeyDown(KeyCode.V) && gameManager.GetLevelCompleteState())
        {
            LevelHealth(currHealth);
            gameManager.OpenDoorState(true);
        }
    }


    void ChangeColor()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            sprite.color = new Color(1, 1, 1, 0.6f);
        }
        else sprite.color = new Color(1,1,1,1);
    }

    void AttackEnemy()
    {
        bool hasHorizontalSpeed = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;
        if (waitBetweenAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && !hasHorizontalSpeed)
            {
                anim.Play("attack");
                waitBetweenAttack = startWaitBetweenAttack;
            }
        }
        else
        {
            waitBetweenAttack -= Time.deltaTime;
        }
        
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
     public void DecreaseHealth()
    {
        currHealth -= 10;
    }
    public void LevelHealth(int health)
    {
        levelHealth = health;
    }
}
