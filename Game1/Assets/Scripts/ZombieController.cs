using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    int count = 0;
    AttackPlayer attackPlayer;
    GameManager gameManager;
    [Header("Health")]
    int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] HealthBar healthBar;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] float distanceFromPlayer;
    Animator zombieAnimator;
    Canvas zombieCanvas;

    [Header("Distance From Player")]
    [SerializeField] float minimumDistToAttackPlayer = 1f;
    [SerializeField] float range = 10f;

    [Header("Speed")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float initialSpeed = 3f;
    Rigidbody2D body;

    [Header("Bools")]
    [SerializeField] bool deadEnd = false;
    [SerializeField] bool hasWalkingSpeed = false;
    public bool isDead = false;
    [SerializeField] bool bossZombie = false;
    public bool callMakeSplat = false;

    [Header("Attack")]
    [SerializeField] float waitBetweenAttack;
    [SerializeField] float nextAttack;
    [SerializeField] int takeDamage = 20;

    [Header("Sounds")]
    AudioSource audioSource = null;
    [SerializeField] AudioClip attackSound = null;
    [SerializeField] float attackVolme = 0.3f;
    [Header("Particles")]
    public ParticleSystem blood = null;
    [SerializeField] GameObject healthCapsule = null;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        attackPlayer = GetComponentInChildren<AttackPlayer>();
        gameManager = FindObjectOfType<GameManager>();
        zombieCanvas = GetComponentInChildren<Canvas>();
        zombieAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!gameManager.GetPlayerDeadState() && !isDead)
        {
            distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
            hasWalkingSpeed = GetWalkingState();
            Attack();
            zombieAnimator.SetBool("isWalking", hasWalkingSpeed);
            MoveZombie();
        }
        IsDying();
    }

    bool GetWalkingState()
    {
        if (distanceFromPlayer > range || deadEnd || distanceFromPlayer < minimumDistToAttackPlayer || Mathf.Abs(transform.position.y - player.transform.position.y) > 2.5)
        {
            return false;
        }
        return true;
    }

    void MoveZombie()
    {
        if (distanceFromPlayer <= range && distanceFromPlayer > minimumDistToAttackPlayer - 0.5f && Mathf.Abs(transform.position.y - player.transform.position.y) <= 2.5)
        {
            
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector2(-1f, 1f);
                body.velocity = new Vector2(-moveSpeed, 0);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
                body.velocity = new Vector2(moveSpeed, 0);
            }
        }
        else
        {
            return;
        }
    }

    void Attack()
    {
        if (waitBetweenAttack < 0)
        {
            if (distanceFromPlayer <= minimumDistToAttackPlayer && Mathf.Abs(transform.position.y - player.transform.position.y) < 2)
            {
                attackPlayer.hasGivenDamage = false;
                zombieAnimator.Play("attack");
                audioSource.PlayOneShot(attackSound, attackVolme);
                waitBetweenAttack = nextAttack;
            }
        }
        else
        {
            waitBetweenAttack -= Time.deltaTime;
        }
        
    }
    public void TakeDamage()
    {
        if (!isDead)
        {
            currentHealth -= takeDamage;
            healthBar.SetHealth(currentHealth);
        }
    }
    void IsDying()
    {
        if (currentHealth <= 0)
        {
            count++;
            zombieCanvas.enabled = false;
            zombieAnimator.Play("dead");
            isDead = true;
            SplashController.instance.MakeSplat();
            StartCoroutine(WaitForDying());
        }
    }
    public void PlayBloodParticleEffect()
    {
        callMakeSplat = true;
        SplashController.instance.MakeSplat();
        callMakeSplat = false;
        blood.Play();
    }
    IEnumerator WaitForDying()
    {
        yield return new WaitForSeconds(1.5f);
        attackPlayer.hasGivenDamage = false;
        // if (!bossZombie)
        // {
        //     PlayerController.currHealth += 5;
        // }
        // else
        // {
        //     PlayerController.currHealth += 10;
        // }

        Instantiate(healthCapsule, transform.position, transform.rotation);

        Destroy(gameObject);

        gameManager.totalEnemy -= 1;

    }
    public bool IsThisABossZombie()
    {
        return bossZombie;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("range") || collision.CompareTag("Box")) return;
        moveSpeed = 0;
        deadEnd = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("range") || collision.CompareTag("Box")) return;
        moveSpeed = initialSpeed;
        deadEnd = false;
    }
}
