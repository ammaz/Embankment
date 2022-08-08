using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    #region singleton

    public static HotbarManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    public Button[] HotbarSlotButtons;
    public List<GameObject> objectList = new List<GameObject>();
    public GameObject[] HotbarObjects = new GameObject[5];

    // Start is called before the first frame update
    void Start()
    {
        SetUpHotBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpHotBar()
    {
        //Debug.Log(Inventory.instance.hotbarItemList);
        for (int a = 0; a < Inventory.instance.hotbarItemList.Count; a++)
        {
            HotbarSlotButtons[a].image.sprite = Inventory.instance.hotbarItemList[a].icon;
            HotbarObjects[a] = objectList.Find(obj => obj.name == Inventory.instance.hotbarItemList[a].name);
        }
    }
}
