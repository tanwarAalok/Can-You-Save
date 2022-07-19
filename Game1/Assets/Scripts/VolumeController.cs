using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeController : MonoBehaviour
{
    [SerializeField] GameObject sliderGameobject = null;
    [SerializeField] Slider slider = null;
    [SerializeField] Sprite muteSprite = null;
    [SerializeField] Sprite unmuteSprite = null;
    [SerializeField] AudioSource audioSource = null;

    Image spriteManager;
    bool toggle = false;


    private void Start() {
        spriteManager = GetComponent<Image>();

    }

    private void Update() {

        audioSource.volume = slider.value;

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
