using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI NPCDialogueText;
    public GameObject dialogueOptionsContainer;  // Grid with the dialogue option buttons
    public GameObject dialogueOptionButtonPrefab;

    protected List<GameObject> dialogueOptionButtons; // List with the options related to the current dialogue
    private GameObject firstOptionButton;

    public NPCInteraction npcInteraction = null;

    public static DialogueUI instance;

    private void Awake()
    {
        if(instance != null) {
            Debug.LogWarning("Multiple instances of DialogueUI found!");
            return;
        }
        instance = this;

        dialogueOptionButtons = new List<GameObject>();

        gameObject.SetActive(false);
    }

    public void OpenDialogueWindow()
    {
        if(!gameObject.activeInHierarchy) {
            gameObject.SetActive(true);
            GameManager.instance.playerInput.SwitchCurrentActionMap("Dialogue");
        }

        ClearDialogueOptions();
    }

    public void CloseDialogueWindow()
    {
        if(gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
            GameManager.instance.playerInput.SwitchCurrentActionMap("Gameplay");
        }
    }

    public void WriteDialogue(NPCDialogue NPCDialogue)
    {
        OpenDialogueWindow();
        NPCDialogueText.text = NPCDialogue.text;

        foreach(PlayerDialogue playerDialogue in NPCDialogue.dialogueOptions) {
            // Instanciate a button for every dialogue option
            GameObject dialogueOptionButtonCopy = Instantiate(dialogueOptionButtonPrefab, dialogueOptionsContainer.transform);
            dialogueOptionButtonCopy.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.text;
            
            // Add the instanciated button reference to the dialogue options list
            dialogueOptionButtons.Add(dialogueOptionButtonCopy);

            // The actual button component in the game object
            Button dialogueOptionButtonComponent = dialogueOptionButtonCopy.GetComponent<Button>();

            /*
             * Insert code to highligth differently the options already picked
             */

            LinkButtonToResponse(dialogueOptionButtonComponent, playerDialogue, NPCDialogue);
        }

        firstOptionButton = dialogueOptionButtons[0];
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstOptionButton);
    }

    private void LinkButtonToResponse(Button dialogueButton, PlayerDialogue playerDialogue, NPCDialogue NPCDialogue)
    {
        NPCQuestInteraction npcQuestInteraction = null;

        if(npcInteraction.GetType() == typeof(NPCQuestInteraction))
            npcQuestInteraction = (NPCQuestInteraction)npcInteraction;

            switch(playerDialogue.dialogueType) {

            case PlayerDialogueType.NextDialogue:
                dialogueButton.onClick.AddListener(delegate { npcInteraction.PlayDialogue(NPCDialogue.nextDialogue); });
                break;

            case PlayerDialogueType.GetInfo:
                dialogueButton.onClick.AddListener(delegate { npcInteraction.PlayDialogue(playerDialogue.nextDialogue); });
                break;

            case PlayerDialogueType.Accept:
                if(!npcQuestInteraction) {
                    Debug.Log("NPCQuestInteraction not found, there shouldn't be an Accept option");
                }
                else {
                    dialogueButton.onClick.AddListener(delegate { 
                        npcQuestInteraction.PlayDialogue(playerDialogue.nextDialogue);
                        npcQuestInteraction.QuestAccepted();
                    });
                }
                break;

            case PlayerDialogueType.Refuse:
                if(!npcQuestInteraction) {
                    Debug.Log("NPCQuestInteraction not found, there shouldn't be a Refuse option");
                }
                else {
                    dialogueButton.onClick.AddListener(delegate {
                        npcQuestInteraction.PlayDialogue(playerDialogue.nextDialogue);
                        npcQuestInteraction.QuestRefused();
                    });
                }
                break;

            case PlayerDialogueType.CompleteQuest:
                if(!npcQuestInteraction) {
                    Debug.Log("NPCQuestInteraction not found, there shouldn't be a CompleteQuest option");
                }
                else {
                    dialogueButton.onClick.AddListener(delegate {
                        npcQuestInteraction.PlayDialogue(playerDialogue.nextDialogue);
                        npcQuestInteraction.QuestCompleted();
                    });
                }
                break;

            case PlayerDialogueType.CloseDialogue:
                dialogueButton.onClick.AddListener(delegate { npcInteraction.CloseDialogue(); });
                break;

            default:
                dialogueButton.onClick.AddListener(delegate { npcInteraction.CloseDialogue(); });
                break;
        }
    }

    private void ClearDialogueOptions()
    {
        foreach(GameObject dialogueOptionButton in dialogueOptionButtons) {
            Destroy(dialogueOptionButton);
        }

        dialogueOptionButtons.Clear();
    }

}
