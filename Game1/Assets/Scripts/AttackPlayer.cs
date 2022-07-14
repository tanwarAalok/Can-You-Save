using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] int giveDamage = 10;
    public bool hasGivenDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !hasGivenDamage)
        {
            hasGivenDamage = true;
            playerController.DecreaseHealth(giveDamage);
        }
    }
}
