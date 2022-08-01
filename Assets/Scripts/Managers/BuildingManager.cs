using BitBenderGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    //Objects for hotbar
    public GameObject[] objects;
    public GameObject pendingObject;

    //Position where we want to put our objects
    private Vector3 pos;

    //Where camera is pointing (From this raycast we will be able to know where our player wants to place an object)
    private RaycastHit hit;
    //For letting raycast hit only certain specified layer (It will be ground)
    [SerializeField] private LayerMask layerMask;

    //Object rotate amount variable
    public float rotateAmount;

    //For Grid Snaping System
    public float gridSize;
    public bool gridOn;

    //Check for Object Placement
    public bool canPlace;

    //Material for Object Placement Demonstration
    [SerializeField] public Material[] materials;

    //For Camera
    public MobileTouchCamera gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        gridOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(pendingObject != null)
        {
            if (gridOn)
            {
                pendingObject.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z)
                    );
            }
            else
            {
                pendingObject.transform.position = pos;
            }

            UpdateMaterials();

            //(Subject to change) (Mobile Touch System)
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
        }
    }

    void UpdateMaterials()
    {
        if (canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        if (!canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    //Select object (Need to use it with hotbar for future)
    public void SelectObject(int index)
    {
        Debug.Log("SelectObject");
        gameCamera.enabled = false;
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
        materials[2] = pendingObject.GetComponent<MeshRenderer>().material;
    }

    public void PlaceObject()
    {
        Debug.Log("PlaceObject");
        gameCamera.enabled = true;
        pendingObject.GetComponent<MeshRenderer>().material = materials[2];
        pendingObject = null;
    }

    public void RotateObject()
    {
        Debug.Log("RotateObject");
        gameCamera.enabled = true;
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }

    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if(xDiff > (gridSize / 2))
        {
            pos += gridSize; 
        }
        return pos;
    }
}
