using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance; // Singleton pattern

    private void Awake()
    {
        if(instance != null) {
            Debug.LogWarning("An instance of the Inventory object already exists!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemAdded(Item item);   // List of functions to call together on the trigger of an event
    public OnItemAdded onItemAddedCallback; // Instance of "OnItemChanged" that contains the actual functions to call
    public delegate void OnItemRemoved(Item item);
    public OnItemRemoved onItemRemovedCallback;

    public List<Item> items = new List<Item>(); // Items in inventory
    public int space = 20;

    public bool Add(Item item)
    {
        //if(!item.isDefaultItem) {
            if(items.Count >= space) {
                Debug.Log("Not enough room in the inventory");
                return false;
            }

            items.Add(item);
            if(onItemAddedCallback != null)
                onItemAddedCallback.Invoke(item);
        //}

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if(onItemRemovedCallback != null)
            onItemRemovedCallback.Invoke(item);
    }

}
