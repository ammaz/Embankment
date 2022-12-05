using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarController : MonoBehaviour
{
    //Getting count of all the hotbarslots
    //public int HotbarSlotSize => gameObject.transform.childCount;
    public int HotbarSlotSize = 6;
    //Storing hotbarslots into List
    private List<ItemSlot> hotbarSlots = new List<ItemSlot>();

    //Hotbar Keys to access slots (Subject to change) (For mobile touch system)
    //Here KeyCode.Alpha 1-6 represents keyboard keys (1-6)
    KeyCode[] hotbarKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };

    private void Start()
    {
        SetUpHotbarSlots();
        Inventory.instance.onItemChange += UpdateHotbarUI;
    }

    private void Update()
    {
        //Getting int/number of the key player have pressed
        //Checking which key player has pressed
        for(int i = 0; i < hotbarKeys.Length; i++)
        {
            if (Input.GetKeyDown(hotbarKeys[i]))
            {
                //use item
                hotbarSlots[i].UseItem();
                return;
            }
        }
    }

    //Updating hotbarUI
    private void UpdateHotbarUI()
    {
        //current item count in the hotbar
        int currentUsedSlotCount = Inventory.instance.hotbarItemList.Count;
        for(int i = 0; i < HotbarSlotSize; i++)
        {
            if (i < currentUsedSlotCount)
            {
                //Adding item to hotbarslot
                hotbarSlots[i].AddItem(Inventory.instance.hotbarItemList[i]);
            }
            else
            {
                //Clearing hotbarslot
                hotbarSlots[i].ClearSlot();
            }
        }
    }

    //Populating hotbar Slots
    private void SetUpHotbarSlots()
    {
        for(int i = 0; i < HotbarSlotSize; i++)
        {
            ItemSlot slot = gameObject.transform.GetChild(i).GetComponent<ItemSlot>();
            hotbarSlots.Add(slot);
        }
    }
}
