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

    //For Description Panel Position (To remove Flickering) (Subject to change) (ForTouchSystem)
    public float moveX = 0f;
    public float moveY = 0f;

    void Update()
    {
        //To add Item in our inventory
        if (Input.GetKeyDown(KeyCode.X))
        {
            Inventory.instance.AddItem(itemList[Random.Range(0, itemList.Count)]);
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
}
