using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenericInteraction : NPCInteraction 
{
    private int lastDialogueIndex = -1;

    protected override void Awake()
    {
        base.Awake();

        dialogueUI = DialogueUI.instance;

        // Reset all isLastPlayed variable in NPC dialogues to false
        foreach(NPCDialogue dialogue in dialogues) {
            dialogue.isLastPlayed = false;
        }
    }

    public override void Interact()
    {
        base.Interact();
        PlayRandomDialogue();
    }

    protected void PlayRandomDialogue()
    {
        int newDialogueIndex = Random.Range(0, dialogues.Count);

        if(dialogues[newDialogueIndex].isLastPlayed) {
            PlayRandomDialogue();
            return;
        }

        if(lastDialogueIndex != -1) {
            dialogues[lastDialogueIndex].isLastPlayed = false;
        }
        lastDialogueIndex = newDialogueIndex;

        PlayDialogue(dialogues[newDialogueIndex]);
    }
}