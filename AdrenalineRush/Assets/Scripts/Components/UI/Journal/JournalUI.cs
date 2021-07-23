using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalUI : MonoBehaviour
{
    public GameObject questLog;
    public GameObject questEntryPrefab;

    private List<QuestUI> questsUI;

    private Journal journal;

    private void Awake()
    {
        journal = Journal.instance;
        journal.onNewQuestAddedCallback += AddNewQuestUI;
        journal.onQuestCompletedCallback += CompleteQuestUI;

        questsUI = new List<QuestUI>();

        gameObject.SetActive(false);
    }

    public void AddNewQuestUI(Quest quest)
    {
        QuestUI currentQuestUI = Instantiate(questEntryPrefab, questLog.transform).GetComponent<QuestUI>();
        currentQuestUI.name.text = quest.name;
        currentQuestUI.description.text = quest.description;

        questsUI.Add(currentQuestUI);
    }

    public void CompleteQuestUI(Quest quest)
    {
        foreach(QuestUI current in questsUI) {
            if(current.name.text == quest.name) {
                Destroy(current.gameObject);
                return;
            }
        }
    }

    public void OpenCloseJournal(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
