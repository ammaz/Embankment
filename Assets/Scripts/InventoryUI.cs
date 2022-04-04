using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    //To check if iventory is open or not
    private bool inventoryOpen = false;

    //Getter => which will return the inventoryOpen value
    public bool InventoryOpen => inventoryOpen;

    [Header("Inventory UI")]
    [SerializeField] public GameObject inventoryParent;
    [SerializeField] public GameObject inventoryTab;
    [SerializeField] public GameObject craftingTab;

    //This will store references of item slots that are currently in use
    private List<ItemSlot> itemSlotList = new List<ItemSlot>();
    public GameObject itemSlotPrefab;
    public Transform inventoryItemTransform;

    //Crafting Recipies
    public Transform craftingItemTransform;

    // Start is called before the first frame update
    void Start()
    {
        //Passing references so UpdateInventoryUI doesn't return null exception
        Inventory.instance.onItemChange += UpdateInventoryUI;
        UpdateInventoryUI();
        SetUpCraftingRecipies();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetUpCraftingRecipies()
    {
        List<Item> craftingRecipes = GameManager.instance.craftingRecipies;

        foreach(Item recipe in craftingRecipes)
        {
            //Adding Recipie to crafting tab
            GameObject Go = Instantiate(itemSlotPrefab, craftingItemTransform);
            ItemSlot slot = Go.GetComponent<ItemSlot>();
            slot.AddItem(recipe);
        }
    }

    //This function will be called everytime we add,remove or craft item
    private void UpdateInventoryUI()
    {
        //Current items count in our inventory
        int currentItemCount = Inventory.instance.inventoryItemList.Count;

        //Check if we have enough slot for items to display in our inventory
        if (currentItemCount > itemSlotList.Count)
        {
            //Add more item slots
            AddItemSlots(currentItemCount);
        }

        //Looking for every single slot available in our inventory and adding items to that slot
        for(int i = 0; i < itemSlotList.Count; ++i)
        {
            if (i < currentItemCount)
            {
                //update the current item in the slot
                itemSlotList[i].AddItem(Inventory.instance.inventoryItemList[i]);
            }
            //If we have too many item slot or deleted items we are destroying inventory slots.
            else
            {
                itemSlotList[i].DestroySlot();
                itemSlotList.RemoveAt(i);
            }
        }
    }

    private void AddItemSlots(int currentItemCount)
    {
        int amount = currentItemCount - itemSlotList.Count;

        for(int i = 0; i < amount; ++i)
        {
            GameObject GO = Instantiate(itemSlotPrefab, inventoryItemTransform);
            ItemSlot newSlot = GO.GetComponent<ItemSlot>();
            itemSlotList.Add(newSlot);
        }
    }

    public void OpenInventory()
    {
        inventoryOpen = true;
        inventoryParent.SetActive(true);
    }

    public void CloseInventory()
    {
        OnInvetoryTabClicked();
        inventoryOpen = false;
        inventoryParent.SetActive(false);
    }

    public void OnCraftingTabClicked()
    {
        craftingTab.SetActive(true);
        inventoryTab.SetActive(false);
    }

    public void OnInvetoryTabClicked()
    {
        craftingTab.SetActive(false);
        inventoryTab.SetActive(true);
    }
}
