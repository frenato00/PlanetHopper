using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public PlayerUI playerUI;

    public Animator animator;
    private TMP_Text nameText;
    private TMP_Text dialogueText;
    private Queue<string> sentences;
    private bool enabled = false;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }


    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        enabled = true;
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    private void Update()
    {
        if (playerUI == null)
        {
            playerUI = FindObjectOfType<PlayerUI>();
            animator = playerUI.dialogueBoxAnimator;
            nameText = playerUI.dialogueName;
            dialogueText = playerUI.dialogueText;
        }
        if (enabled)
        {
            if (Input.GetButtonDown("Interact"))
            {
                DisplayNextSentence();
                Debug.Log("space bar");
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        // dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log(dialogueText.text);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        enabled = false;
        animator.SetBool("IsOpen", false);

    }

}
