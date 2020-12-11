using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public Dialogue dialogue;
    Queue<string> sentences;

    public GameObject dialoguePanel;
    public TextMeshProUGUI displayText;
    public GameObject npcContainerImage;
    public Sprite npcImage;

    string activeSentence;
    public float typingSpeed;

    AudioSource myAudio;
    public AudioClip speakSound;

    Image componentImage;
    
    

    bool insidePlayer = false;
    bool change = false;
    string sceneName = "";

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        myAudio = GetComponent<AudioSource>();
        componentImage = npcContainerImage.GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) & insidePlayer)
        {
            DisplayNextSentence();
        }
    }

        void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        componentImage.sprite = npcImage;
        insidePlayer = true;
        sentences.Clear();

        foreach(string sentence in dialogue.sentenceList)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void changeMap(bool chang, string sceneNam)
    {
        change = chang;
        sceneName = sceneNam;
        dialoguePanel.SetActive(true);
        componentImage.sprite = npcImage;
        insidePlayer = true;
        sentences.Clear();

        foreach (string sentence in dialogue.sentenceList)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if(sentences.Count <= 0)
        {
            displayText.text = activeSentence;
            dialoguePanel.SetActive(false);
            if (change)
            {
                SceneManager.LoadScene(sceneName);
            }
            return;
        }
        activeSentence = sentences.Dequeue();
        displayText.text = activeSentence;

        StopAllCoroutines();
        StartCoroutine(TypeTheSentence(activeSentence));
    }

    IEnumerator TypeTheSentence(string sentence)
    {
        displayText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            displayText.text += letter;
            myAudio.PlayOneShot(speakSound);
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            StartDialogue();            
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
            insidePlayer = false;
        }
    }
}
