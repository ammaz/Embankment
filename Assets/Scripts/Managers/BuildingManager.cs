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

    //For Grid Snaping System
    public float gridSize;
    public bool gridOn;

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
            
            //(Subject to change) (Mobile Touch System)
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
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

    public void SelectObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation);
    }

    public void PlaceObject()
    {
        pendingObject = null;
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
