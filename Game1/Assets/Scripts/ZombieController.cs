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
    bool isDead;
    [SerializeField] float minimumDistToAttack = 2f;
    [SerializeField]float range = 10f;
    [SerializeField]float speed = 10f;
    Rigidbody2D body;
    

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
        distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
        if(distanceFromPlayer < minimumDistToAttack && !isDead && playerController.AttackEnemy())
        {
            StartCoroutine(TakeDamage(20));
        }

        if(currentHealth <= 0){
            zombieCanvas.enabled = false;
            zombieAnimator.Play("dead");
            isDead = true;
            StartCoroutine(IsDying());
        }

        
        if(!isDead)
        {
            AttackPlayer();

            if(distanceFromPlayer < range && distanceFromPlayer > 1.9)
            {
                MoveZombie();
                zombieAnimator.Play("walk");
            }
        }
    }
    IEnumerator TakeDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator IsDying()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void MoveZombie()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1f, 1f);
            body.velocity = new Vector2(-speed, 0);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
            body.velocity = new Vector2(speed, 0);
        }
    }


    public bool AttackPlayer()
    {
        if(distanceFromPlayer < minimumDistToAttack)
        {
            zombieAnimator.Play("attack");
            StartCoroutine(WaitForPlayerDamage());
            return true;
        }
        return false;
    }
    IEnumerator WaitForPlayerDamage()
    {
        yield return new WaitForSeconds(1f);
    }
}
