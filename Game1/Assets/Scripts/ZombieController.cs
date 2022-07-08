using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject player;
    PlayerController playerController;
    [SerializeField] float distanceFromPlayer;
    Animator zombieAnimator;
    Canvas zombieCanvas;
    
 

    bool isDead = false;

    [SerializeField] float minimumDistToAttack = 2f;
    [SerializeField] float range = 10f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float initialSpeed = 3f;
    [SerializeField]bool hasWalkingSpeed = false;
    [SerializeField]bool deadEnd = false;
    Rigidbody2D body;

    float nextAttack = 0f;


    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        zombieCanvas = GetComponentInChildren<Canvas>();
        zombieAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerController.gameOver)
        {
            distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
            TakeDamage();
            IsDying();
            hasWalkingSpeed = GetWalkingState();
            if(!isDead)
            {
                if (Time.time > nextAttack)
                {
                    AttackPlayer();
                    nextAttack = Time.time + 1f;
                }
                zombieAnimator.SetBool("isWalking",hasWalkingSpeed);
                MoveZombie();
            }
        }

    }
    bool GetWalkingState()
    {
        if(distanceFromPlayer > range || deadEnd || distanceFromPlayer < minimumDistToAttack)
        {
            return false;
        }
        return true;    
    }

    void MoveZombie()
    {
        if (distanceFromPlayer <= range && distanceFromPlayer > 1.5)
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
        if(distanceFromPlayer < minimumDistToAttack)
        {
            zombieAnimator.Play("attack");
            StartCoroutine(WaitForPlayerDamage());
            
        }
    }
    IEnumerator WaitForPlayerDamage()
    {
        yield return new WaitForSeconds(0.8f);
        playerController.currHealth -= 10;
    }

    void TakeDamage()
    {
        if (distanceFromPlayer < minimumDistToAttack && !isDead && playerController.AttackEnemy())
        {
            StartCoroutine(WaitToTakeDamage(20));
        }
    }
    IEnumerator WaitToTakeDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) return;
        moveSpeed = 0;
        deadEnd = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) return;
        moveSpeed = initialSpeed;
        deadEnd = false;
    }
}
