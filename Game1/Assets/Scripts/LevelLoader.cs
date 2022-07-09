using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        if(gameManager.GetPlayerState())
        {
            StartCoroutine(ReloadTime(SceneManager.GetActiveScene().buildIndex));
        }

        if(gameManager.LevelComplete())
        {
            StartCoroutine(ReloadTime(SceneManager.GetActiveScene().buildIndex+1));
        }
    }
    IEnumerator ReloadTime(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
