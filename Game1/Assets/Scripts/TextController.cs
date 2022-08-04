using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TextController : MonoBehaviour
{
    GameManager gameManager;
    [Header("Text Field")]
    [SerializeField] GameObject textBox = null;
    [SerializeField] TextMeshProUGUI showText = null;
    [SerializeField] TextMeshProUGUI dialogueText = null;
    [SerializeField] GameObject dialogueBox = null;
    public bool dialogeRunning = false;
    bool isVPressed = false;
    static int dialogueRunCount = 0;
    static int levelDialogueCount = 0;
    [SerializeField] AudioSource sparksSound = null;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueRunCount = levelDialogueCount;
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Dialogues("Welcome brave warrior,\n You have been chosen to defend mankind from the\n ZOMBIE APOCALPYSE");
        }
        Debug.Log(dialogueRunCount);
    }
    private void Update()
    {
        if(gameManager.GetPlayerDeadState())
        {
            TotalDialoguesCompleted(dialogueRunCount);
            Debug.Log(dialogueRunCount);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dialogueBox != null)
        {
            if (collision.CompareTag("Faulty") && !dialogeRunning && dialogueRunCount == 1)
            {
                Dialogues("Looks like this switch is not working properly \n These boxes are disappearing....");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("faultySwitchRange") && !sparksSound.isPlaying) sparksSound.Play();
        
        if (textBox != null)
        {
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
    IEnumerator TypeSentence(string sentence,int sentenceLength)
    {
        dialogueText.text = "";
        for (int item = 0; item < sentenceLength; item++)
        {
            dialogueText.text += sentence[item];
            yield return new WaitForSeconds(0.025f);
        }
        dialogeRunning = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(sparksSound!=null)
        {
            sparksSound.Stop();
        }
        if (textBox != null)
        {
            textBox.SetActive(false);
            FindObjectOfType<PlayerController>().canOpenDoor = false;
            isVPressed = false;
        }
        if(dialogueBox!=null)
        {
            dialogueBox.SetActive(false);
        }
    }
    public void Dialogues(string sentence)
    {
        dialogueRunCount++;
        TotalDialoguesCompleted(dialogueRunCount);
        dialogeRunning = true;
        dialogueBox.SetActive(true);
        int sentenceLength = sentence.Length;
        StartCoroutine(TypeSentence(sentence, sentenceLength));
    }
    public void TotalDialoguesCompleted(int dialogueRunCount)
    {
        levelDialogueCount = dialogueRunCount;
    }


}
