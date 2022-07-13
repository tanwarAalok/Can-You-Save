using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject doorLocked = null;
    [SerializeField] GameObject doorOpened = null;
    [SerializeField] GameObject doorUnlocked = null;
    public int totalEnemy;
    public GameObject[] enemies;
    bool isPlayerDead = false;
    bool openDoor = false;
    bool isLevelComplete = false;

    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemy = enemies.Length;
    }
    void Update() {
        
        if(SceneManager.GetActiveScene().buildIndex < 2)
        {
            if(totalEnemy <= 0)
            {
                isLevelComplete = true;
                doorLocked.SetActive(false);
                doorUnlocked.SetActive(true);
            }

            if(openDoor)
            {
                doorUnlocked.SetActive(false);
                doorOpened.SetActive(true);
            }
        }
    }
    public void PlayerDeadState(bool check)
    {
        isPlayerDead = check;
    }
    public bool GetPlayerDeadState()
    {
        return isPlayerDead;
    }
    public bool GetLevelCompleteState()
    {
        return isLevelComplete;
    }
    public void OpenDoorState(bool check)
    {
        openDoor = check;
    }
    public bool LevelComplete()
    {
        return openDoor;
    }

}
