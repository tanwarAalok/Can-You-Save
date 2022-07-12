using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameManager gameManager;
    [Header("Health")]
    int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] HealthBar healthBar;

    [Header("Player")]
    [SerializeField] GameObject player;
    PlayerController playerController;
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
    [SerializeField] bool isDead = false;

    [Header("Attack")]
    [SerializeField] float waitBetweenAttack;
    [SerializeField] float nextAttack;
    
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
        zombieCanvas = GetComponentInChildren<Canvas>();
        zombieAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!gameManager.GetPlayerDeadState())
        {
            distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
            hasWalkingSpeed = GetWalkingState();
            if (!isDead)
            {
                if (waitBetweenAttack<=0)
                {
                    AttackPlayer();
                    waitBetweenAttack = nextAttack;
                }
                else
                {
                    waitBetweenAttack -= Time.deltaTime;
                }
                zombieAnimator.SetBool("isWalking", hasWalkingSpeed);
                MoveZombie();
            }
        }
        IsDying();

    }
    bool GetWalkingState()
    {
        if(distanceFromPlayer > range || deadEnd || distanceFromPlayer < minimumDistToAttackPlayer)
        {
            return false;
        }
        return true;    
    }

    void MoveZombie()
    {
        if (distanceFromPlayer <= range && distanceFromPlayer > 0.8f)
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

    void AttackPlayer()
    {
        if(distanceFromPlayer <= minimumDistToAttackPlayer && Mathf.Abs(transform.position.y - player.transform.position.y) < 1)
        {
            zombieAnimator.Play("attack");
        }
    }

    public void TakeDamage()
    {
        if (!isDead)
        {
            currentHealth -= 20;
            healthBar.SetHealth(currentHealth);
        }
    }
    void IsDying()
    {
        if (currentHealth <= 0)
        {
            zombieCanvas.enabled = false;
            zombieAnimator.Play("dead");
            isDead = true;
            StartCoroutine(WaitForDying());
        }
    }
    IEnumerator WaitForDying()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        gameManager.totalEnemy -= 1;

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("range")) return;
        moveSpeed = 0;
        deadEnd = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("range")) return;
        moveSpeed = initialSpeed;
        deadEnd = false;
    }
}
