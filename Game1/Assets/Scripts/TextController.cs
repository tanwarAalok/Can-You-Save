using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TextController : MonoBehaviour
{
    [SerializeField] GameObject playerUI;
    [Header("Text Field")]
    [SerializeField] GameObject textBox = null;
    [SerializeField] TextMeshProUGUI showText = null;
    [SerializeField] GameObject dialogueBox = null;
    [SerializeField] TextMeshProUGUI dialogueText = null;

    [Header("Dialogue Field")]
    [TextArea(2, 6)]
    [SerializeField] List<string> dialogueSentences = new List<string>();
    public bool dialogeRunning = false;
    bool isVPressed = false;
    public static int totalDialoguesCompleted = 0;
    [SerializeField] float waitToChangeDialogue = 2.5f;
    int numberOfDialoguesToRun = 0;

    [Header("Sound Field")]
    [SerializeField] AudioSource sparksSound = null;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && totalDialoguesCompleted<3 && dialogueBox!=null)
        {
            numberOfDialoguesToRun = 3;
            Dialogues(numberOfDialoguesToRun);
        }
    }
    private void Update()
    {
        if(dialogueBox.activeSelf)
        {
            playerUI.SetActive(false);
        }
        else if(!dialogueBox.activeSelf)
        {
            playerUI.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dialogueBox != null)
        {
            if (collision.CompareTag("Faulty") && !dialogeRunning && totalDialoguesCompleted == 3)
            {
                numberOfDialoguesToRun = 1;
                Dialogues(numberOfDialoguesToRun);
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
    IEnumerator TypeSentence(string sentence,int sentenceLength, int k)
    {
        dialogueText.text = "";
        for (int item = 0; item < sentenceLength; item++)
        {
            dialogueText.text += sentence[item];
            yield return new WaitForSeconds(0.025f);
        }
        k--;
        yield return new WaitForSeconds(waitToChangeDialogue);
        dialogeRunning = false;
        Dialogues(k);
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
    }
    public void Dialogues(int k)
    {
        if(!dialogeRunning && k!=0)
        {
            string sentence = dialogueSentences[totalDialoguesCompleted];
            totalDialoguesCompleted++;
            dialogeRunning = true;
            dialogueBox.SetActive(true);
            int sentenceLength = sentence.Length;
            StartCoroutine(TypeSentence(sentence, sentenceLength, k));
        }
        
        if(k==0)
        {
            dialogeRunning = false;
            dialogueBox.SetActive(false);
        }
    }
}
