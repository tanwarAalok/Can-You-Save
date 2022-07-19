using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource audioSource;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    public void PauseAudio()
    {
        audioSource.Pause();
    }
    public void PlayAudio()
    {
        audioSource.UnPause();
    }
    public void Volume(float volume)
    {
        audioSource.volume = volume;
    }
}
