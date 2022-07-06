using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject player;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = transform.position.x - player.transform.position.x;
        if(distanceFromPlayer < 2 && Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(TakeDamage(20));
        }

        if(currentHealth <= 0){
            Destroy(this.gameObject);
        }
    }

    IEnumerator TakeDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
