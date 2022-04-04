using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Prefab with this name will be built (These will be our Stat Items)
[CreateAssetMenu(fileName = "StatItem", menuName = "Item/StatItem")]

//Using Inheritence (StatItem is inherited from Item Class) (That means it can access all Item Class Functions and Modify them if allowed)
public class StatItem : Item
{
    //This will display in StatItem Type
    public StatItemType itemType;
    //Item amount we want to add
    public int amount;

    public override void Use()
    {
        base.Use();
        //Using Items that we have crafted
        GameManager.instance.OnStatItemUse(itemType, amount);
        /*
        //After using that Item we can remove it aswell
        Inventory.instance.RemoveItem(this);
        */
    }
}

public enum StatItemType
{
    Soldier,
    Vehicle,
    Trap,
    Construction,
    Building
}
