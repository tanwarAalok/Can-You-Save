using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashController : MonoBehaviour
{
    GameManager gameManager;
    public static SplashController instance;
    public GameObject[] splat;
    [SerializeField] float offsetY = 0.9f;
    [SerializeField] List<GameObject> enemies;
    // Start is called before the first frame update
    void Awake()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        instance = this;
        gameManager = FindObjectOfType<GameManager>();
    }
    public void MakeSplat()
    {
        Vector3 offset = new Vector3(0, -offsetY, 0); 
        if(gameManager.GetPlayerDeadState())
        {
            var newSplat = Instantiate(splat[Random.Range(0, splat.Length)], FindObjectOfType<PlayerController>().transform.position + offset, transform.rotation).GetComponent<SpriteRenderer>();
            newSplat.color = new Color(1,0,0,1);
        }
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].GetComponent<ZombieController>().isDead)
                {
                    Instantiate(splat[Random.Range(0, splat.Length)], enemies[i].transform.position + offset, transform.rotation);
                    enemies.Remove(enemies[i]);
                    break;
                }
                if(enemies[i].GetComponent<ZombieController>().callMakeSplat && !enemies[i].GetComponent<ZombieController>().isDead)
                {
                    Instantiate(splat[Random.Range(0, splat.Length)], enemies[i].transform.position + offset, transform.rotation);
                }
            }
        }
    }
}
