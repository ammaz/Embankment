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
    [SerializeField] public GameObject hotbarTab;

    //This will store references of item slots that are currently in use
    private List<ItemSlot> itemSlotList = new List<ItemSlot>();


    public GameObject inventorySlotPrefab;
    public GameObject craftingSlotPrefab;

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
            GameObject Go = Instantiate(craftingSlotPrefab, craftingItemTransform);
            CraftingSlot slot = Go.GetComponent<CraftingSlot>();
            slot.AddItem(recipe);

            CraftingRecipe craftRecipe = (CraftingRecipe)recipe;
            craftRecipe.parentCraftingSlot = slot;
        }
    }

    //This function will be called everytime we add,remove or craft item
    private void UpdateInventoryUI()
    {
        //Current items count in our inventory
        //int currentItemCount = Inventory.instance.inventoryItemList.Count;
        
        int currentItemCount = 0;

        for (int a = 0; a < Inventory.instance.inventoryItemList.Count; a++)
        {
            if (Inventory.instance.inventoryItemList[a].name == "Oil" || Inventory.instance.inventoryItemList[a].name == "Money")
            {
                continue;
            }
            else
            {
                currentItemCount++;
            }
        }

        //Check if we have enough slot for items to display in our inventory
        if (currentItemCount > itemSlotList.Count)
        {
            //Add more item slots
            AddItemSlots(currentItemCount);
        }
        /*
        //Looking for every single slot available in our inventory and adding items to that slot
        for(int i = 0; i < itemSlotList.Count; ++i)
        {
            if (i < currentItemCount)
            {
                itemSlotList[i].AddItem(Inventory.instance.inventoryItemList[i]);
            }
            //If we have too many item slot or deleted items we are destroying inventory slots.
            else
            {
                itemSlotList[i].DestroySlot();
                itemSlotList.RemoveAt(i);
            }
            
        }
        */

        int invCount = 0;

        //Looking for every single slot available in our inventory and adding items to that slot
        for (int i = 0; i < Inventory.instance.inventoryItemList.Count; ++i)
        {
            if (!(Inventory.instance.inventoryItemList[i].name == "Oil" || Inventory.instance.inventoryItemList[i].name == "Money"))
            {
                if(invCount < currentItemCount)
                {
                    itemSlotList[invCount].AddItem(Inventory.instance.inventoryItemList[i]);
                    invCount++;
                }
            }
        }
        //If we have too many item slot or deleted items we are destroying inventory slots
        for (int i = 0; i < itemSlotList.Count; ++i)
        {
            if (!(i < currentItemCount))
            {
                itemSlotList[i].DestroySlot();
                itemSlotList.RemoveAt(i);
            }
        }

        //Counting number of Oil and Money
        MainMenu.instance.CountMoneyOrOil();
    }
    //Here I can add item stack system
    private void AddItemSlots(int currentItemCount)
    {
        int amount = currentItemCount - itemSlotList.Count;

        for(int i = 0; i < amount; ++i)
        {
            GameObject GO = Instantiate(inventorySlotPrefab, inventoryItemTransform);
            ItemSlot newSlot = GO.GetComponent<ItemSlot>();
            itemSlotList.Add(newSlot);
        }
    }

    public void OpenInventory()
    {
        inventoryOpen = true;
        inventoryParent.SetActive(true);
        hotbarTab.SetActive(true);
    }

    public void CloseInventory()
    {
        OnInvetoryTabClicked();
        inventoryOpen = false;
        inventoryParent.SetActive(false);
        hotbarTab.SetActive(false);
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
