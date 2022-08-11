using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public GameObject selectedObject;
    public List<GameObject> selectPlayers = new List<GameObject>();
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
                else if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Select(hit.collider.gameObject);
                }
                else if (selectedObject!=null && selectedObject.CompareTag("Player"))
                {
                    //selectedObject.GetComponent<PlayerAI>().walkPoint = hit.point;
                    //selectedObject.GetComponent<PlayerAI>().walkPointSet = true;

                    foreach(GameObject o in selectPlayers)
                    {
                        if (o != null)
                        {
                            o.GetComponent<PlayerAI>().walkPoint = hit.point;
                            o.GetComponent<PlayerAI>().walkPointSet = true;
                        }       
                    }

                    Deselect();
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
        if (obj.CompareTag("Object"))
        {
            //For Objects
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

        else if (obj.CompareTag("Player"))
        {
            //For Player
            buildingManager.gameCamera.enabled = false;
            if (obj == selectedObject) return;
            obj.GetComponent<PlayerAI>().playerSelectUI.SetActive(true);
            selectedObject = obj;
            selectPlayers.Add(selectedObject);
        }
        
    }

    public void Deselect()
    {
        if (selectedObject.CompareTag("Player"))
        {
            buildingManager.gameCamera.enabled = true;
            //selectedObject.GetComponent<PlayerAI>().playerSelectUI.SetActive(false);
            //selectedObject = null;

            foreach (GameObject o in selectPlayers)
            {
                if(o != null)
                    o.GetComponent<PlayerAI>().playerSelectUI.SetActive(false);
            }

            selectPlayers.Clear();
            selectedObject = null;
        }
        else
        {
            buildingManager.gameCamera.enabled = true;
            objUI.SetActive(false);
            selectedObject.GetComponent<Outline>().enabled = false;
            selectedObject = null;
        }
    }

    public void Move()
    {
        buildingManager.gameCamera.enabled = false;
        buildingManager.pendingObject = selectedObject;
        buildingManager.materials[2] = selectedObject.GetComponent<MeshRenderer>().material;
    }

    public void Rotate()
    {
        buildingManager.gameCamera.enabled = true;
        selectedObject.transform.Rotate(Vector3.up, buildingManager.rotateAmount);
    }

    public void Delete()
    {
        buildingManager.gameCamera.enabled = true;
        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy.GetComponent<HealthManager>().HealthBarSlider.gameObject);
        Destroy(objToDestroy);
    }
}
