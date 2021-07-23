using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuestGiver))]
public class NPCQuestInteraction : NPCInteraction
{
    private QuestGiver questGiver;  // Component with which the NPC can give quests to the player

    // TODO Make NPCs have multiple quests
    // public List<Quest> quests;
    public Quest currentQuest;

    protected override void Awake()
    {
        base.Awake();

        questGiver = GetComponent<QuestGiver>();
        currentQuest.Initialize();

        // For when NPC will have multiple quests
        // currentQuest = null;
    }

    public override void Interact()
    {
        base.Interact();

        if(currentQuest.isActive) {
            if(currentQuest.totalAmountReached)     
                PlayDialogue(dialogues[2]);         // Dialogue to complete the quest
            else                                    
                PlayDialogue(dialogues[1]);         // Dialogue for when the quest is already given
        }
        else if(currentQuest.isCompleted) {         
            PlayDialogue(dialogues[3]);             // Dialogue for when the quest is already completed
        }
        else {
            PlayDialogue(dialogues[0]);             // Dialogue for when the quest is not yet given
        }

    }

    public void QuestAccepted()
    {
        questGiver.GiveQuest(currentQuest);
    }

    public void QuestRefused()
    {
        Debug.Log("The quest was refused");
    }

    public void QuestCompleted()
    {
        questGiver.CompleteQuest(currentQuest);
    }

    /*

    public override void Interact()
    {
        if(!currentQuest.isActive) {
            // Player accepts quest
            QuestAccepted();
        }

        if(currentQuest.isCompleted) {
            Debug.Log("The quest is completed!");
        }





        // TODO Interact override for NPCQuestInteraction

        /*
         * If first interaction check for quests
         * If there are quests related to the NPC, open dialogue box and print quest dialogue
         * On new interaction check if there are more dialogues to the quest, if so print them
         * On last interaction close dialogue box
         * 
         * If there are no quests related to the NPC, open dialogue box and print random dialogue
         * On new interaction close dialogue box
         * 
         */

    /*

    if(interactionPrompt.activeInHierarchy) {
        base.Interact();
    }

    // Second or more interaction
    if(dialogueIsActive) {
        if(currentDialogues != null) {
            if(currentDialogueIndex < currentDialogues.Count) {
                PlayDialogue(currentDialogues[currentDialogueIndex]);
                return;
            }
            else {

                CloseDialogue();
                return;
            }
        }
        else {
            CloseDialogue();
            return;
        }

    }


    // First interaction

    DialogueBox.SetActive(true);
    dialogueIsActive = true;

    if(questGiver != null && quests.Count > 0) {
        Debug.Log(quests[0].name);

        currentQuest = quests[0];
        currentDialogues = currentQuest.questOfferDialogues;
        currentDialogueIndex = 0;
        PlayDialogue(currentDialogues[currentDialogueIndex]);
    }
    else {
        PlayRandomDialogue();
    }





    */

    /*
    }

    public void QuestAccepted()
    {
        questGiver.GiveQuest(currentQuest);
    }

    public override void PlayNextDialogue()
    {
        PlayDialogue(lastDialogueIndex + 1);
    }
    */
}
