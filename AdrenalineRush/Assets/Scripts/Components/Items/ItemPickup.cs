using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    Inventory inventory;
    public Item item;

    private void Awake()
    {
        inventory = Inventory.instance;
    }

    public override void Interact()
    {
        base.Interact();
        inventory.Add(item);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        interactionPromptUI.interactionType.text = "Pick up";    // Interaction text of the prompt
    }
}
