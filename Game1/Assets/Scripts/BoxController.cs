using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxController : MonoBehaviour
{
    [SerializeField] GameObject addBox = null;
    [SerializeField] GameObject player;
    bool canSwitchOn = false;
    [SerializeField] Sprite GreenSwitch = null;
    [SerializeField] GameObject[] removeBoxes = null;
    [SerializeField] bool isBossZombiePresent = false;
    [SerializeField] bool wasBossZombieEverPresent = false;
    private void Update() {
        if(FindObjectOfType<ZombieController>()!=null)
        {
            isBossZombiePresent = FindObjectOfType<ZombieController>().IsThisABossZombie();
            if(isBossZombiePresent ==true)
            {
                wasBossZombieEverPresent = true;
            }
        }
        else
        {
            isBossZombiePresent = false;
        }
        ButtonTrigger();
    }

    void ButtonTrigger()
    {
        if(canSwitchOn && Input.GetKeyDown(KeyCode.X) || (!isBossZombiePresent && wasBossZombieEverPresent))
        {
            if(addBox!=null)
            {
                addBox.SetActive(true);
            }
            for (int i = 0; i < removeBoxes.Length; i++)
            {
                removeBoxes[i].SetActive(false);
            }
            if(GreenSwitch!=null)
            {
                transform.GetComponent<SpriteRenderer>().sprite = GreenSwitch;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
    
        canSwitchOn = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        canSwitchOn = false;
    }
}
