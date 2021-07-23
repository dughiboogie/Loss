using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerDialogueType {
    NextDialogue,
    GetInfo,
    Accept,
    Refuse,
    CompleteQuest,
    CloseDialogue
}

[CreateAssetMenu(fileName = "PlayerDialogue_", menuName = "Dialogues/Player dialogue")]
public class PlayerDialogue : ScriptableObject
{
    [TextArea(15, 20)] public string text;
    public PlayerDialogueType dialogueType;

    public NPCDialogue nextDialogue;
    public bool alreadyPlayed;
}
