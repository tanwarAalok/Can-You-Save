using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchController : MonoBehaviour
{
    public GameObject box;
    public GameObject player;
    bool canSwitchOn = false;
    public Sprite GreenSwitch;
    public GameObject SwitchText;


    private void Update() {
        ButtonTrigger();
    }

    void ButtonTrigger()
    {
        if(canSwitchOn && Input.GetKeyDown(KeyCode.X))
        {
            box.SetActive(true);
            transform.GetComponent<SpriteRenderer>().sprite = GreenSwitch;

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
    
        canSwitchOn = true;
        SwitchText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        canSwitchOn = false;
        SwitchText.SetActive(false);
    }
}
