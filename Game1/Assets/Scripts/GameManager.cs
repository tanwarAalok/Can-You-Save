using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
    using UnityEditor;
#endif
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
    public GameObject pauseMenu = null;
    public GameObject gameWonMenu = null;
    public bool isPaused = false;
    public bool gameWon = false;

    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemy = enemies.Length;
    }
    void Update() {

        if(gameWon)
        {
            gameWonMenu.SetActive(true);
            Time.timeScale = 0;
        }

        if(!gameWon && !isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            AudioManager.instance.PauseAudio();
            pauseMenu.SetActive(isPaused);
            Time.timeScale = 0;
        }
        
        if(SceneManager.GetActiveScene().buildIndex < 3)
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
    public void PauseMenu()
    {
        isPaused = false;
        AudioManager.instance.PlayAudio();
        pauseMenu.SetActive(isPaused);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
