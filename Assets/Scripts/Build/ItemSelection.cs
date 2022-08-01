using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public GameObject selectedObject;
    public Text objNameText;
    private BuildingManager buildingManager;
    public GameObject objUI;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //(Subject to change) (Mobile Touch System)
        //This method will point a ray to the center of the camera and select the object
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000))
            {
                //Checking if the object tag is correct or not
                if (hit.collider.gameObject.CompareTag("Object"))
                {
                    Select(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedObject!=null)
        {
            Deselect();
        }
    }

    private void Select(GameObject obj)
    {
        Debug.Log("SelectItemSelection");
        buildingManager.gameCamera.enabled = false;
        if (obj == selectedObject) return;
        if (selectedObject != null) Deselect();
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null)
        {
            obj.AddComponent<Outline>();
            obj.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        }
        else
        {
            outline.enabled = true;
        }

        //objNameText.text = obj.name;
        objNameText.text = obj.name.Substring(0, obj.name.IndexOf('('));
        objUI.SetActive(true);
        selectedObject = obj;
    }

    private void Deselect()
    {
        Debug.Log("DeselectItemSelection");
        buildingManager.gameCamera.enabled = true;
        objUI.SetActive(false);
        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
    }

    public void Move()
    {
        Debug.Log("MoveItemSelection");
        buildingManager.gameCamera.enabled = false;
        buildingManager.pendingObject = selectedObject;
        buildingManager.materials[2] = selectedObject.GetComponent<MeshRenderer>().material;
    }

    public void Rotate()
    {
        Debug.Log("RotateItemSelection");
        buildingManager.gameCamera.enabled = true;
        selectedObject.transform.Rotate(Vector3.up, buildingManager.rotateAmount);
    }

    public void Delete()
    {
        Debug.Log("DeleteItemSelection");
        buildingManager.gameCamera.enabled = true;
        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy);
    }
}
