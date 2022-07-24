using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
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
    public bool canOpenDoor = false;
    public bool isSwitchOn = true;

    [Header("Attack")]
    [SerializeField] float waitBetweenAttack;
    [SerializeField] float startWaitBetweenAttack;

    [Header("Sounds")]
    [SerializeField] AudioClip attackSound = null;
    [SerializeField] AudioClip[] runSound = null;
    [SerializeField] AudioClip jumpSound = null;
    [SerializeField] float audioVolume = 0.7f;
    [SerializeField] float jumpVolume = 1f;
    AudioSource audioSource;

    [Header("Particle Effects")]
    [SerializeField] ParticleSystem runParticles;
    [SerializeField] Text death;

    [Header("Death Related")]
    public static int deathCount = 0;
    [SerializeField] GameObject antidote = null;
    [SerializeField] Sprite emptyVessel = null;
    bool callDeathCount = false;

    [Header("Global Volume")]
    float currIntensity;


    private void Awake() 
    {
        currIntensity = 0.3f;
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
        death.text = "Deaths: " + deathCount.ToString();
        if(deathCount > 5 && antidote!=null)
        {
            antidote.GetComponent<SpriteRenderer>().sprite = emptyVessel;
        }
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
        if (currHealth <= 0 || feetCollider.IsTouchingLayers(LayerMask.GetMask("Obstacle")))
        {
            anim.Play("dead");
            if(!callDeathCount)
            {
                deathCount++;
                callDeathCount = true;
            }
            death.text = "Deaths: " + deathCount.ToString();
            body.velocity = new Vector2(0, body.velocity.y);
            gameOver = true;
            gameManager.PlayerDeadState(gameOver);
            SplashController.instance.MakeSplat();
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
                audioSource.PlayOneShot(attackSound, audioVolume);
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

        if(hasHorizontalSpeed && (feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Box")))) {
            if(!bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
            {
                runParticles.Play();
            }
            if(!audioSource.isPlaying && !gameManager.isPaused)
            { 
                int randomIdx = Random.Range(0, runSound.Length);
                audioSource.PlayOneShot(runSound[randomIdx], audioVolume);
            }
        }
        else
        {
            runParticles.Stop();
        }
    }


    void Jump()
    {
        // Jumping
        if ((feetCollider.IsTouchingLayers(LayerMask.GetMask("groundLayer")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Box"))) && Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity += new Vector2(0, jumpSpeed);
            anim.Play("Jump");
            audioSource.PlayOneShot(jumpSound, jumpVolume);
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
        PostProcessController.instance.VignetteColor();
        float percentageDecrease = (damage * 100) / levelHealth;
        float change = (percentageDecrease * 1) / 100;
        currIntensity += change;
        PostProcessController.instance.VignetteIntensity(currIntensity);
    }

    public void LevelHealth(int health)
    {
        levelHealth = health;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Collectiable"))
        {
            if (deathCount > 5)
            {
                other.gameObject.GetComponent<SpriteRenderer>().sprite = emptyVessel;
            }
            gameManager.gameWon = true;
            Destroy(other.gameObject);
        }

    }

    
}
