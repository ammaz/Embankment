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

    //This list will hold all the items of player hotbar
    public List<Item> hotbarItemList = new List<Item>();
    public HotbarController hotbarController;

    //This function will take care for switching the item (We will send the item that we want to switch to from inventory to hotbar)
    public void SwitchHorbarInventory(Item item)
    {
        //Inventory to hotbar
        foreach(Item i in inventoryItemList)
        {
            if (i == item)
            {
                //If we dont have enough space in hotbar
                if (hotbarItemList.Count >= hotbarController.HotbarSlotSize)
                {
                    Debug.Log("No more slots available in hotbar");
                }
                else
                {
                    //Adding item to hotbar
                    hotbarItemList.Add(item);
                    //Remove item from inventory
                    inventoryItemList.Remove(item);
                    //Updating inventory (To avoid null reference)
                    onItemChange.Invoke();
                }
                return;
            }
        }

        //hotbar to inventory
        foreach(Item i in hotbarItemList)
        {
            if(i == item)
            {
                hotbarItemList.Remove(item);
                inventoryItemList.Add(item);
                onItemChange.Invoke();
                return;
            }
        }
    }

    public void AddItem(Item item)
    {
        inventoryItemList.Add(item);
        //Calling  UpdateInventoryUI(); function
        onItemChange.Invoke();
    }

    public void RemoveItem(Item item)
    {
        if (inventoryItemList.Contains(item))
        {
            inventoryItemList.Remove(item);
        }
        else if (hotbarItemList.Contains(item))
        {
            hotbarItemList.Remove(item);
        }

        onItemChange.Invoke();
    }

    //This method will be called when user wants to craft an item (It will check if he has the required ingredients or not to craft it)
    public bool ContainsItem(string itemName,int amount)
    {
        int itemCounter = 0;

        foreach(Item i in inventoryItemList)
        {
            if (i.name == itemName)
            {
                itemCounter++;
            }
        }

        foreach (Item i in hotbarItemList)
        {
            if (i.name == itemName)
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

    public void RemoveItems(string itemName,int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            RemoveItemType(itemName);
        }
    }

    public void RemoveItemType(string itemName)
    {
        foreach (Item i in inventoryItemList)
        {
            if (i.name == itemName)
            {
                inventoryItemList.Remove(i);
                return;
            }
        }

        foreach (Item i in hotbarItemList)
        {
            if (i.name == itemName)
            {
                hotbarItemList.Remove(i);
                return;
            }
        }
    }
}
