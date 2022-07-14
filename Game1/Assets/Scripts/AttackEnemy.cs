using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public bool hasGivenDamage = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !hasGivenDamage)
        {
            hasGivenDamage = true;
            collision.GetComponent<ZombieController>().TakeDamage();
        }
    }
}
