using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    private Item item;
    //Is an item being dragged by player?
    public bool isBeingDraged = false;

    //Returning current Item
    public Item Item => item;

    //Using virtual funtion so that it can be modified in derived class aswell
    public virtual void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
    }

    //Clearing slot when the item will be destroyed/used
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
    }

    public void UseItem()
    {
        if (item == null || isBeingDraged == true) return;   

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            //Debug.Log("Trying to switch");
            Inventory.instance.SwitchHorbarInventory(item);
        }
        else
        {
            item.Use();
        }   
    }

    public void DestroySlot()
    {
        Destroy(gameObject);
    }

    public void OnRemoveButtonClicked()
    {
        if (item != null)
        {
            Inventory.instance.RemoveItem(item);
        }
    }

    //Mouse hower system to display item info (Subject to change) (ForTouchSystem)
    public void OnCursorEnter()
    {
        //If there is no item in the slot this function will return
        if (item == null || isBeingDraged == true) return;
        //Display item info
        GameManager.instance.DisplayItemInfo(item.name, item.GetItemDescription(), transform.position);
    }

    public void OnCursorExit()
    {
        //If there is no item in the slot this function will return
        if (item == null) return;
        //Display item info exit
        GameManager.instance.DestroyItemInfo();
    }
}
