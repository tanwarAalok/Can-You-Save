using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    AttackEnemy attackEnemy;

    [Header("Speed")]
    [SerializeField] float runSpeed = 5;
    [SerializeField] float jumpSpeed = 10;
    private Animator anim;
    SpriteRenderer sprite;

    [Header("Health Bar")]
    [SerializeField] HealthBar healthBar;
    public static int currHealth = 200;
    static int levelHealth = 200;
    [SerializeField] int maxHealth = 200;

    [Header("Game Manager")]
    GameManager gameManager;
    [SerializeField] bool gameOver = false;
    bool canOpenDoor = false;

    [Header("Attack")]
    [SerializeField] float waitBetweenAttack;
    [SerializeField] float startWaitBetweenAttack;

    [Header("Text Field")]
    [SerializeField] GameObject textBox = null;
    [SerializeField] TextMeshProUGUI showText = null;


    public AudioClip attackSound = null;
    public AudioClip[] runSound = null;
    public AudioClip jumpSound = null;
    AudioSource audioSource = null;

    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
        attackEnemy = FindObjectOfType<AttackEnemy>();
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
            healthBar.SetHealth(currHealth);
            if (SceneManager.GetActiveScene().buildIndex <3)
            {
                OpenDoor();
            }
            if(feetCollider.IsTouchingLayers(LayerMask.GetMask("Zombie")) && !feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")))
            {
                float moveALitte = 0.02f * transform.localScale.x;
                transform.position = new Vector2(transform.position.x + moveALitte, transform.position.y);
            }
        }

        ChangeColor();
        if(currHealth <= 0 || feetCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle"))) {
            anim.Play("dead");
            body.velocity = new Vector2(0, body.velocity.y);
            gameOver = true;
            gameManager.PlayerDeadState(gameOver);
        }
    }
    
    void OpenDoor()
    {
        if (canOpenDoor && Input.GetKeyDown(KeyCode.V))
        {
            if (gameManager.GetLevelCompleteState())
            {
                LevelHealth(currHealth);
                gameManager.OpenDoorState(true);
            }
            else
            {
                if(textBox!=null)
                {
                    textBox.SetActive(true);
                    showText.color = new Color(1, 0.1f, 0.2f);
                    showText.text = "All Zombies are not Dead! You cannot get through";
                }
                else
                {
                    return;
                }
            }
        }
    }


    void ChangeColor()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle")))
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
                attackEnemy.hasGivenDamage = false;
                anim.Play("attack");
                audioSource.PlayOneShot(attackSound, 0.6f);
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

        if(hasHorizontalSpeed && !audioSource.isPlaying && (feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Box")))) {

            int randomIdx = Random.Range(0, runSound.Length);
            audioSource.PlayOneShot(runSound[randomIdx], 0.6f);
        }
    }


    void Jump()
    {
        // Jumping
        if ((feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Box"))) && Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity += new Vector2(0, jumpSpeed);
            anim.Play("Jump");
            audioSource.PlayOneShot(jumpSound, 0.8f);
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
     public void DecreaseHealth(int damage)
    {
        currHealth -= damage;
    }
    public void LevelHealth(int health)
    {
        levelHealth = health;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Color orignalColor = new Color(0.724f, 1, 0, 1);
        if(textBox!=null)
        {
            showText.color = orignalColor;
            if(collision.CompareTag("Orignal_Player"))
            {
                textBox.SetActive(true);
                showText.text = "Orignal Character Sprite";
            }
            else if(collision.CompareTag("Switch"))
            {
                textBox.SetActive(true);
                showText.text = "Press 'X' to Switch On";
            }
            else if (collision.CompareTag("Door"))
            {
                canOpenDoor = true;
                textBox.SetActive(true);
                showText.text = "Press 'V' to Open Door";
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(textBox!=null)
        {
            textBox.SetActive(false);
            canOpenDoor = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Collectiable"))
        {
            gameManager.gameWon = true;
            Destroy(other.gameObject);
        }
    }
}
