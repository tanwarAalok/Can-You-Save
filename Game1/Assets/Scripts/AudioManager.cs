using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip mainmenuMusic  = null;
    [SerializeField] AudioClip gameMusic = null;
    GameManager gameManager = null;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {

        if(SceneManager.GetActiveScene().buildIndex == 0 || (gameManager != null && gameManager.isPaused)) {
            audioSource.clip = mainmenuMusic;
        }
        else {
            audioSource.clip = gameMusic;
        }

        if(!audioSource.isPlaying) audioSource.Play();
    }


}
