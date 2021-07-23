using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Component to attach to game objects that can give quests to the player (e.g. NPCs, items, etc.)
 * 
 */
public class QuestGiver : MonoBehaviour
{
    private Journal journal;

    private void Awake()
    {
        journal = Journal.instance;
    }

    public void GiveQuest(Quest quest)
    {
        journal.AddNewQuest(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        journal.CompleteQuest(quest);
    }
}
