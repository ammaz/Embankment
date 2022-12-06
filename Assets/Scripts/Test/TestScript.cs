using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Button[] hotbarButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Debug.Log(Inventory.instance.hotbarItemList);
            for(int a = 0; a < Inventory.instance.hotbarItemList.Count; a++)
            {
                //Debug.Log(Inventory.instance.hotbarItemList[a].name);
                hotbarButton[a].image.sprite=Inventory.instance.hotbarItemList[a].icon;
            }
        }
    }
}
