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
}
