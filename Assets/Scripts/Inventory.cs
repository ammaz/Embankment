using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    //A Delegate is a reference pointer to a method. It allows us to treat method as a variable and pass method as a variable for a callback. 
    public delegate void OnItemChange();

    //This will be called when we invoke the functions of craft, hold or remove items
    public OnItemChange onItemChange = delegate { };

    //This list will hold all the items of player inventory
    public List<Item> inventoryItemList = new List<Item>();

    public void AddItem(Item item)
    {
        inventoryItemList.Add(item);
        //Calling  UpdateInventoryUI(); function
        onItemChange.Invoke();
    }

    public void RemoveItem(Item item)
    {
        inventoryItemList.Remove(item);
        onItemChange.Invoke();
    }

    //This method will be called when user wants to craft an item (It will check if he has the required ingredients or not to craft it)
    public bool ContainsItem(Item item,int amount)
    {
        int itemCounter = 0;

        foreach(Item i in inventoryItemList)
        {
            if (i == item)
            {
                itemCounter++;
            }
        }

        if (itemCounter >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItems(Item item,int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            RemoveItem(item);
        }
    }
}
