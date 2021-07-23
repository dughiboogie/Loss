using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : Interactable
{
    public List<NPCDialogue> dialogues;  // Different dialogue lines not connected to each other

    public DialogueUI dialogueUI;

    protected override void Awake()
    {
        base.Awake();

        dialogueUI = DialogueUI.instance;
    }

    public override void Interact()
    {
        base.Interact();
    }

    public void PlayDialogue(NPCDialogue dialogue)
    {
        dialogueUI.WriteDialogue(dialogue);
        dialogue.isLastPlayed = true;
    }

    public void CloseDialogue()
    {
        dialogueUI.CloseDialogueWindow();
        interactionPromptUI.EnableInteractionPrompt();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        interactionPromptUI.interactionType.text = "Speak with";    // Interaction text of the prompt
        dialogueUI.npcInteraction = this;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        dialogueUI.CloseDialogueWindow();
    }

}
