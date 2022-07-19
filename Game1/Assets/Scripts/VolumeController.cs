using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VolumeController : MonoBehaviour
{
    public static float currentVolume = - 1;
    [SerializeField] GameObject sliderGameobject = null;
    [SerializeField] Slider slider = null;
    [SerializeField] Sprite muteSprite = null;
    [SerializeField] Sprite unmuteSprite = null;
    [SerializeField] AudioSource audioSource = null;

    Image spriteManager;
    bool toggle = false;


    private void Start() {

        spriteManager = GetComponent<Image>();
        if(currentVolume != -1)
        {
            slider.value = currentVolume;
            audioSource.volume = slider.value;
        }
    }

    private void Update() {
        currentVolume = slider.value;
        audioSource.volume = slider.value;
        if(SceneManager.GetActiveScene().buildIndex !=0)
        {
            AudioManager.instance.Volume(currentVolume);
        }
        if(slider.value == 0) {
            spriteManager.sprite = muteSprite;
        }
        else spriteManager.sprite = unmuteSprite;
    }

    public void Toggle()
    {
        toggle = !toggle;
        sliderGameobject.SetActive(toggle);
    }

}
