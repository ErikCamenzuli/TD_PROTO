using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private float clickCooldown = 0.2f; 
    private float nextClickTime = 0f;

    void Update()
    {
        //Check if right-click is pressed and the cooldown has passed
        if (Input.GetMouseButtonDown(1) && Time.time >= nextClickTime)
        {
            Debug.Log("Right-click detected, starting raycast.");
            DetectRightClickOnBuilding();
            nextClickTime = Time.time + clickCooldown; 
        }
    }

    //Method to detect if right-clicked on a building with a BuildingSellHandler
    private void DetectRightClickOnBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Raycast hit: " + hitObject.name + " (Full path: " + GetFullGameObjectPath(hitObject) + ")");

            //Try to find BuildingSellHandler on the hit object or its parent
            BuildingSellHandler sellHandler = hitObject.GetComponent<BuildingSellHandler>();
            if (sellHandler == null)
            {
                //Check the parent if the component is not on the hit object itself
                sellHandler = hitObject.GetComponentInParent<BuildingSellHandler>();
            }

            if (sellHandler != null)
            {
                Debug.Log("BuildingSellHandler found on: " + sellHandler.gameObject.name);
                sellHandler.SellBuilding(); 
            }
            else
            {
                Debug.Log("No BuildingSellHandler component found on hit object or its parent.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any objects.");
        }
    }

    private string GetFullGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;

        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }

        return path;
    }
}
