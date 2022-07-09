using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject doorLocked;
    [SerializeField] GameObject doorOpened;
    [SerializeField] GameObject doorUnlocked;
    bool isPlayerDead = false;
    public int totalEnemy;
    public GameObject[] enemies;
    public bool openDoor = false;

    private void Start() 
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemy = enemies.Length;

        
    }
    public void PlayerDeadState(bool check)
    {
        isPlayerDead = check;
    }
    public bool GetPlayerState()
    {
        return isPlayerDead;
    }

    private void Update() {

        Debug.Log(totalEnemy);
        
        if(totalEnemy <= 0)
        {
            doorLocked.SetActive(false);
            doorUnlocked.SetActive(true);
        }

        if(openDoor)
        {
            doorUnlocked.SetActive(false);
            doorOpened.SetActive(true);
        }
    }

    public bool LevelComplete()
    {
        return openDoor;
    }

    


}
