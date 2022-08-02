using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextController : MonoBehaviour
{
    [Header("Text Field")]
    [SerializeField] GameObject textBox = null;
    [SerializeField] TextMeshProUGUI showText = null;
    bool isVPressed = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (textBox != null)
        {
            if(collision.CompareTag("Faulty")){
                textBox.SetActive(true);
                showText.text = "Opps!! Looks like, this switch is not working properly \n These boxes are disappearing....";
            }

            if (collision.CompareTag("Orignal_Player"))
            {
                textBox.SetActive(true);
                showText.text = "Orignal Character Sprite";
            }
            else if (collision.CompareTag("Switch"))
            {
                textBox.SetActive(true);
                if (collision.GetComponent<SwitchController>().isSwitchOn)
                {
                    showText.text = "Switch is already On";
                }
                else
                {
                    showText.text = "Press 'X' to Switch On";
                }
            }
            else if (collision.CompareTag("Door"))
            {
                textBox.SetActive(true);
                if(FindObjectOfType<GameManager>().totalEnemy > 0 && Input.GetKeyDown(KeyCode.V))
                {
                    isVPressed = true;
                    showText.color = new Color(1, 0.1f, 0.2f);
                    showText.text = "All Zombies are not Dead! You cannot get through";
                }

                if(!isVPressed)
                {
                    Color orignalColor = new Color(0.724f, 1, 0, 1);
                    showText.color = orignalColor;
                    showText.text = "Press 'V' to Open Door";
                    FindObjectOfType<PlayerController>().canOpenDoor = true;
                }
                
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (textBox != null)
        {
            textBox.SetActive(false);
            FindObjectOfType<PlayerController>().canOpenDoor = false;
            isVPressed = false;
        }
    }

}
