using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public List<Item> itemList = new List<Item>();
    public List<Item> craftingRecipies = new List<Item>();

    //For Description Panel
    public Transform canvas;
    public GameObject itemInfoPrefab;
    private GameObject currentItemInfo = null;

    /*//For Description Panel Position (To remove Flickering) (Subject to change) (ForTouchSystem)
    public float moveX = 0f;
    public float moveY = 0f;*/

    public Transform mainCanvas;
    public Transform hotbarTransform;
    public Transform inventoryTransform;

    void Start()
    {
        AddMoneyItemToInventory(100);
        AddOilItemToInventory(50);
        MainMenu.instance.CountMoneyOrOil();
    }

    void Update()
    {
        //To add Item in our inventory (Subject to change) (For mobile touch system)
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Instantiating new item just to add different reference in memory thats why I have used newItem variable here
            Item newItem = itemList[Random.Range(0, itemList.Count)];
            Inventory.instance.AddItem(Instantiate(newItem));

            //Inventory.instance.AddItem(itemList[Random.Range(0, itemList.Count)]);
        }
    }

    //Whenever player will use the item this message will be displayed
    public void OnStatItemUse(StatItemType itemType, int amount)
    {
        Debug.Log("Consuming " + itemType + " Add amount: " + amount);
    }

    //I will add touch position here and replace Vector2 buttonPos (Subject to change) (ForTouchSystem)
    public void DisplayItemInfo(string itemName, string itemDescription, Vector2 buttonPos)
    {
        if(currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }

        buttonPos.x -= 270;
        buttonPos.y += 120;

        //Using Quaternion so that object can't rotate (Gets aligned with the world or parent axes)
        currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, canvas);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription);
    }

    public void DestroyItemInfo()
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }
    }

    public void AddItemToInventory()
    {
        //Instantiating new item just to add different reference in memory thats why I have used newItem variable here
        Item newItem = itemList[Random.Range(0, itemList.Count)];
        Inventory.instance.AddItem(Instantiate(newItem));

        //Counting number of Oil and Money
        MainMenu.instance.CountMoneyOrOil();
    }

    //Adding Money to Inventory
    public void AddMoneyItemToInventory(int amount)
    {
        for(int a = 0; a < amount; a++)
        {
            //0 index for Money, change it if you add/delete index of items in your gameManager object (Subject to change)
            Inventory.instance.AddItem(itemList[0]);
        }
    }
    //Adding Oil to Inventory
    public void AddOilItemToInventory(int amount)
    {
        for (int a = 0; a < amount; a++)
        {
            //0 index for Money, change it if you add/delete index of items in your gameManager object (Subject to change)
            Inventory.instance.AddItem(itemList[1]);
        }
    }
}
