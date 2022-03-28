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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
