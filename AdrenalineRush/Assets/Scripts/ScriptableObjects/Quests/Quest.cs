using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New quest", menuName = "Quests/Quest")]
[System.Serializable]
public class Quest : ScriptableObject
{
    [TextArea(15, 20)] public string description;

    public bool isActive = false;
    public bool totalAmountReached = false;
    public bool isCompleted = false;

    // Variable to keep track of the quest's objective
    public int totalAmount;
    protected int currentAmount;

    public virtual void Initialize()
    {
        isActive = false;
        totalAmountReached = false;
        isCompleted = false;
        currentAmount = 0;
    }

    public void Evaluate()
    {
        if(currentAmount >= totalAmount) {
            totalAmountReached = true;
        }
    }

    public void Complete()
    {
        if(isActive && totalAmountReached) {
            isActive = false;
            isCompleted = true;
        }
    }

}
