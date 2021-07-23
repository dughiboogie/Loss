using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New gathering quest", menuName = "Quests/Gathering quest")]
public class GatheringQuest : Quest
{
    public QuestItem itemToGather;

    public override void Initialize()
    {
        Debug.Log("Gathering quest Initialize");

        base.Initialize();

        Inventory.instance.onItemAddedCallback += CheckQuestItemAdded;
        Inventory.instance.onItemRemovedCallback += CheckQuestItemRemoved;
    }

    private void CheckQuestItemAdded(Item item)
    {
        QuestItem questItem = (QuestItem)item;

        if(questItem && questItem.quest.name == name) {

            Debug.Log("Quest item " + item.name + " added to inventory.");

            currentAmount++;
            Evaluate();
        }
    }

    private void CheckQuestItemRemoved(Item item)
    {
        QuestItem questItem = (QuestItem)item;

        if(questItem && questItem.quest.name == name) {
            currentAmount--;
            Evaluate();
        }
    }
}
