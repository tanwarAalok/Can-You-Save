using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider2D;
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
    [SerializeField] bool isDead = false;
    [SerializeField] bool bossZombie = false;

    [Header("Attack")]
    [SerializeField] float waitBetweenAttack;
    [SerializeField] float nextAttack;
    [SerializeField] int takeDamage = 20;

    void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        attackPlayer = FindObjectOfType<AttackPlayer>();
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
            if (waitBetweenAttack <= 0)
            {
                attackPlayer.hasGivenDamage = false;
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

    void AttackPlayer()
    {
        if (distanceFromPlayer <= minimumDistToAttackPlayer && Mathf.Abs(transform.position.y - player.transform.position.y) < 2)
        {
            zombieAnimator.Play("attack");
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
    public bool IsThisABossZombie()
    {
        return bossZombie;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("range") || capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Box"))) return;
        moveSpeed = 0;
        deadEnd = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("range") || capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Box"))) return;
        moveSpeed = initialSpeed;
        deadEnd = false;
    }
}
