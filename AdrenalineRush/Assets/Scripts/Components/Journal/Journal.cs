using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Journal : MonoBehaviour
{
    public List<Quest> quests;

    public delegate void OnNewQuestAdded(Quest quest);
    public OnNewQuestAdded onNewQuestAddedCallback;
    public delegate void OnQuestCompleted(Quest quest);
    public OnQuestCompleted onQuestCompletedCallback;

    #region Singleton

    public static Journal instance;

    private void Awake()
    {
        if(instance != null) {
            Debug.LogWarning("Multiple instances of Journal found!");
            return;
        }
        instance = this;
    }

    #endregion

    public void AddNewQuest(Quest quest)
    {
        quest.isActive = true;
        quests.Add(quest);

        // add questEntry to UI
        if(onNewQuestAddedCallback != null) {
            onNewQuestAddedCallback.Invoke(quest);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        quest.Complete();

        //remove questEntry from UI
        if(onQuestCompletedCallback != null) {
            onQuestCompletedCallback.Invoke(quest);
        }
    }
}
