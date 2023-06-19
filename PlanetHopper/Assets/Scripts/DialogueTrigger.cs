using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool isCollider;
    public Dialogue dialogue;

    public void TriggerDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerStay(Collider other) {
        if(isCollider && other.CompareTag("Player") && FindObjectOfType<DialogueManager>().playerUI!=null){
            TriggerDialogue();
            this.gameObject.SetActive(false);
        } 
    }
}
