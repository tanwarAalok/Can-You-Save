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

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        zombieCanvas = GetComponentInChildren<Canvas>();
        zombieAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = transform.position.x - player.transform.position.x;
        if(distanceFromPlayer < 2 && !isDead && playerController.Attack())
        {
            StartCoroutine(TakeDamage(20));
        }

        if(currentHealth <= 0){
            zombieCanvas.enabled = false;
            zombieAnimator.Play("dead");
            isDead = true;
        }
    }

    IEnumerator TakeDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
