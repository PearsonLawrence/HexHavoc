//written by braden turner
//controlls how the text is displayed for the player, allows skipping thru, and writes the words line by line

using UnityEngine;
using TMPro;
using System.Collections;

public class subtitleControl : MonoBehaviour
{
    public GameObject dialogueBox; //dialogue box
    public TextMeshProUGUI dialogueText; // uses textmesh pro 2 display
    public string[] dialogueLines; //array of lines 2 display

    public KeyCode skipKey = KeyCode.Space; //key 2 skip (change 2 animation prob @pearson)
    public float textSpeed = 0.1f; //speed of text display

    private int currentLineIndex = 0; 
    private bool isTyping = false; //is typing. no repeats 

    private void Start()
    {
        //hidden 2 start
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        // check if active
        if (dialogueBox.activeSelf && Input.GetKeyDown(skipKey))
        {
            //if typing, skip 2 the end line
            if (isTyping)
            {
                StopAllCoroutines(); 
                dialogueText.text = dialogueLines[currentLineIndex]; //full line
                isTyping = false; //no longer typing
            }
            else
            {
                //go 2 next line 
                currentLineIndex++;

                //all dialogue displayed? 
                if (currentLineIndex >= dialogueLines.Length)
                {
                    //when all displayed, remove box
                    dialogueBox.SetActive(false);
                }
                else
                {
                    //next line
                    StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
                }
            }
        }
    }

    public void StartDialogue()
    {
        //reset dialogue
        currentLineIndex = 0;

        //activate
        dialogueBox.SetActive(true);

        //type 
        StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true; // 
        dialogueText.text = ""; // Clear

        //iterate through text
        foreach (char letter in text)
        {
            dialogueText.text += letter; // add char
            yield return new WaitForSeconds(textSpeed); //wait delay b4 next char
        }

        isTyping = false; 
    }


        public void SkipDialogue()
    {
        // Increment to the next dialogue line
        currentLineIndex++;

        // Check if all dialogue lines have been displayed
        if (currentLineIndex >= dialogueLines.Length)
        {
            // Hide the dialogue box when all lines have been displayed
            dialogueBox.SetActive(false);
        }
        else
        {
            // Display the next dialogue line
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
        }
    }

}
