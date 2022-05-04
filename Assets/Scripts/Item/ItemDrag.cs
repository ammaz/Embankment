using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //(Subject to change) (For mobile touch system)

    private ItemSlot itemSlot;
    //private Transform baseParent; (Deleted)
    private RectTransform hotbarRect;
    //private int siblingIdx; (Deleted)
    private RectTransform inventoryRect;

    //Variables for seleceted item preview
    public GameObject previewPrefab;
    private GameObject currentPreview;
    private Image image;
    private Color baseColor;

    private bool isHotbarSlot;

    // Start is called before the first frame update
    void Start()
    {
        itemSlot = GetComponent<ItemSlot>();
        //baseParent = transform.parent; (Deleted)
        //Passing reference to hotbarRect object (Its taking reference from GameManager script) (Here 'as' keyword is changing its datatype to RectTransform[typecasting])
        hotbarRect = GameManager.instance.hotbarTransform as RectTransform;
        //siblingIdx = transform.GetSiblingIndex(); (Deleted)
        inventoryRect = GameManager.instance.inventoryTransform as RectTransform;

        image = GetComponent<Image>();
        baseColor = image.color;

        //Checking if the dragged object is in hotbar or in inventory when user will release/drop it
        isHotbarSlot = RectTransformUtility.RectangleContainsScreenPoint(hotbarRect, transform.position);
    }

    //Here IDragHandler, IBeginDragHandler and IEndDragHandler are unity interfaces for draging objects

    //This function will be called once, when we begin dragging the item
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Setting new Parent so that the item will be visible through out the screen, it will be not limited to one inventory panel
        //transform.SetParent(GameManager.instance.mainCanvas); (Deleted)
        //Deactivating cursor
        itemSlot.OnCursorExit();
        itemSlot.isBeingDraged = true;

        //Changing alpha of selected item
        var tmpColor = baseColor;
        tmpColor.a = 0.6f;
        image.color = tmpColor;


        //Instantiating new preview of selected item
        currentPreview = Instantiate(previewPrefab, GameManager.instance.mainCanvas);
        //Setting up icon for new preview/Image component
        currentPreview.GetComponent<Image>().sprite = itemSlot.Item.icon;
        //Transforming it to new position
        currentPreview.transform.position = transform.position;
    }

    //This function will be called while we are dragging the item, it will be called every time when we want to update the item position
    public void OnDrag(PointerEventData eventData)
    {
        //Updating item position to Cursor position
        currentPreview.transform.position = Input.mousePosition;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Resetting the transform for that object
        //transform.SetParent(baseParent); (Deleted)
        //transform.SetSiblingIndex(siblingIdx); (Deleted)
        itemSlot.isBeingDraged = false;

        //Resetting alpha of selected item
        image.color = baseColor;

        //This condition is checking if we let go our mouse pointer over hotbar ractangle UI
        if ( (RectTransformUtility.RectangleContainsScreenPoint(hotbarRect, Input.mousePosition) && !isHotbarSlot)
            || (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition) && isHotbarSlot) )
        {
            Inventory.instance.SwitchHorbarInventory(itemSlot.Item);
        }

        //Destroying preview of the selected item
        Destroy(currentPreview);
    }
}
