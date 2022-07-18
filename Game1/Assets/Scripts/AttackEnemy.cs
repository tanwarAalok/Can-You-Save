using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public bool hasGivenDamage = false;
    [SerializeField] float intensity = 0.5f;
    [SerializeField] float shakeTime = 0.5f;
    CinemachineShake cinemachineShake;
    private void Awake()
    {
        cinemachineShake = FindObjectOfType<CinemachineShake>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hasGivenDamage)
        {
            cinemachineShake.ShakeCamera(intensity, shakeTime);
            hasGivenDamage = true;
            collision.GetComponent<ZombieController>().TakeDamage();
            collision.GetComponent<ZombieController>().PlayBloodParticleEffect();
        }
    }
}
