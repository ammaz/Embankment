using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Prefab with this name will be built (These will be our Base Items)
[CreateAssetMenu(fileName = "Item", menuName = "CraftingRecipe/baseRecipe")]
public class CraftingRecipe : Item
{
    //Final Outcome of the object that we have crafted
    public Item result;
    //Ingredients of the item that we have to collect in order to craft it
    public Ingredient[] ingredients;
    //Crating Time
    public float craftTime = 1f;
    //Crafting Slot
    public CraftingSlot parentCraftingSlot;

    private bool CanCraft()
    {
        //Asking the inventory object, if player have enough resources or not to craft the item
        foreach(Ingredient ingredient in ingredients)
        {
            //Here instance is a unique object of class inventory with distinct properites.
            bool containsCurrentIngredient = Inventory.instance.ContainsItem(ingredient.item.name, ingredient.amount);

            if (!containsCurrentIngredient)
            {
                return false;
            }
        }

        return true;
    }

    private void RemoveIngredientsFromInventory()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            Inventory.instance.RemoveItems(ingredient.item.name, ingredient.amount);
        }

    }

    public bool CrafItem()
    {
        if (!CanCraft()) return false;

        //For crafting queue count
        parentCraftingSlot.DecreaseCount();
        //Removing ingredients from inventory
        RemoveIngredientsFromInventory();
        //Start Crafting
        parentCraftingSlot.StartCrafting();

        return true;

    }

    public override void Use()
    {
        //For crafting queue count
        parentCraftingSlot.IncreaseCount();
        //For adding crafting item
        Inventory.instance.AddCraftingItem(this);
    }

    public override string GetItemDescription()
    {
        string itemIngredients = "";

        foreach (Ingredient ingredient in ingredients)
        {
            itemIngredients += "- " + ingredient.amount + " " + ingredient.item.name + "\n";
        }

        return itemIngredients;
    }

    [System.Serializable]
    public class Ingredient
    {
        public Item item;
        public int amount;
    }
}
