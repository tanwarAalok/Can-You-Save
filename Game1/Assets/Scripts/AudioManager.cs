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
        Volume(getVolume());
        
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
    public void StopAudio(bool stopTheAudio)
    {
        if(stopTheAudio)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
    private float getVolume()
    {
        return VolumeController.currentVolume;
    }
}
