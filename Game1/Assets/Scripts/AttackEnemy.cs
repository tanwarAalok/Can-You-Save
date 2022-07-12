using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    ZombieController zombieController;
    private void Start()
    {
        zombieController = FindObjectOfType<ZombieController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            zombieController.TakeDamage();
        }
    }
}
