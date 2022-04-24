using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : ItemSlot
{
    public GameObject craftingImageGO;
    private Image craftingImage;
    private float crafTime;

    //Crafting Queue Text
    public GameObject craftingTextGO;
    private Text craftingText;
    private int currentCount = 0;

    //Overriding/Modifying AddItem() function
    public override void AddItem(Item newItem)
    {
        //Here base.AddItem is calling AddItem() function from ItemSlot
        base.AddItem(newItem);
        craftingImage = craftingImageGO.GetComponent<Image>();
        craftingImage.sprite = newItem.icon;
        craftingImageGO.SetActive(false);

        //Type casting to access craftTime variable
        crafTime = ((CraftingRecipe)newItem).craftTime;

        //For crafting Queue Text
        craftingText = craftingTextGO.GetComponent<Text>();
        craftingTextGO.SetActive(false);
    }

    //For increasing crafting queue count
    public void IncreaseCount()
    {
        currentCount++;
        if (gameObject.activeInHierarchy)
        {
            craftingText.text = currentCount.ToString();
            craftingTextGO.SetActive(true);
        }
    }

    //For decreasing crafting queue count
    public void DecreaseCount()
    {
        currentCount--;
        if(currentCount == 0)
        {
            craftingTextGO.SetActive(false);
        }
        else if (gameObject.activeInHierarchy)
        {
            craftingText.text = currentCount.ToString();
            craftingTextGO.SetActive(true);
        }
    }

    //This function is for, if the user hoop over back to inventory tab then
    //crafting tab will be disabled and if he wants to hoop over back to crafting tab while items are being crafted this function will check
    //if current count is greater than 0 then set it to current count.
    private void OnEnable()
    {
        if(currentCount > 0)
        {
            craftingText.text = currentCount.ToString();
            craftingTextGO.SetActive(true);
        }
        else
        {
            craftingTextGO.SetActive(false);
        }
    }

    //For reseting crafting queue count
    public void ResetCount()
    {
        currentCount = 0;
        craftingTextGO.SetActive(false);
    }

    public void StartCrafting()
    {
        if(gameObject.activeInHierarchy == true)
        {
            StartCoroutine(CraftingAnimation());
        } 
    }

    private IEnumerator CraftingAnimation()
    {
        float timeElapsed = 0f;
        craftingImageGO.SetActive(true);
        craftingImage.fillAmount = 1f;

        while(timeElapsed < crafTime)
        {
            timeElapsed += Time.deltaTime;
            //Lerping btw two values for crafting queue effect
            craftingImage.fillAmount = Mathf.Lerp(1f, 0f, timeElapsed / crafTime);
            yield return null;
        }

        craftingImageGO.SetActive(false);
    }

    private void OnDisable()
    {
        //(Subject to change)
        StopAllCoroutines();
        craftingImageGO.SetActive(false);
    }
}
