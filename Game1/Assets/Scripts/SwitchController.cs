using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchController : MonoBehaviour
{
    [SerializeField] GameObject addBox;
    [SerializeField] GameObject player;
    bool canSwitchOn = false;
    [SerializeField] Sprite GreenSwitch;
    [SerializeField] GameObject SwitchText;
    [SerializeField] GameObject[] removeBoxes;
    [SerializeField] GameObject doorSwitch;


    private void Update() {
        ButtonTrigger();
    }

    void ButtonTrigger()
    {
        if(canSwitchOn && Input.GetKeyDown(KeyCode.X))
        {
            addBox.SetActive(true);
            for (int i = 0; i < removeBoxes.Length; i++)
            {
                removeBoxes[i].SetActive(false);
            }
            transform.GetComponent<SpriteRenderer>().sprite = GreenSwitch;
            doorSwitch.GetComponent<SpriteRenderer>().sprite = GreenSwitch;

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
