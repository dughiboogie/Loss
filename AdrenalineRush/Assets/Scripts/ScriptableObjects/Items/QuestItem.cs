using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New quest item", menuName = "Items/Quest item")]

public class QuestItem : Item
{
    public Quest quest;
}
