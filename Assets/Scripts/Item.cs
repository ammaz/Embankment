using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Prefab with this name will be built (These will be our Base Items)
[CreateAssetMenu(fileName ="Item", menuName ="Item/baseItem")]
public class Item : ScriptableObject
{
    //Using new keyword because in scriptable object a variable with (name) already exists
    new public string name = "Default Item";
    //Prefab Icon
    public Sprite icon = null;

    //Using virtual method because I will expand and add features in it(Override it) in crafting and recipe class 
    public virtual void Use()
    {
        Debug.Log("Using "+ name);
    }
}
