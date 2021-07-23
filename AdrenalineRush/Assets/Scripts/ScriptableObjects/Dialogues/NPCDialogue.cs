using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue_", menuName = "Dialogues/NPC dialogue")]
public class NPCDialogue : ScriptableObject
{
    [TextArea(15, 20)] public string text;
    public bool isLastPlayed = false;

    public List<PlayerDialogue> dialogueOptions;

    public NPCDialogue nextDialogue;
}
