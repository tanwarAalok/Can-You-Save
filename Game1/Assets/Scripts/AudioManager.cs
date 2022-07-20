using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource audioSource;
    float currVolume;

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
        Volume(getVolume());
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSource.Stop();
        }
    }

    public void PauseAudio()
    {
        audioSource.Pause();
    }
    public void PlayAudio()
    {
        audioSource.UnPause();
        Volume(getVolume());
    }
    public void Volume(float volume)
    {
        audioSource.volume = volume;
    }

    private float getVolume()
    {
        return VolumeController.currentVolume;
    }
}
