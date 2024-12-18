using UnityEngine;
using System;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject smallPowerBuildingPrefab;
    public GameObject mediumPowerBuildingPrefab;
    public GameObject largePowerBuildingPrefab;
    public GameObject turretPrefab;
    public GameObject turretPlacementEffect;
    public Vector3 effectsPosition;

    [NonSerialized] public Transform target;
    public float turretRange = 10f;
    public float fireRate = 5f;
    public float fireCountDown = 2.5f;

    public GameObject bulletPrefab;
    public Transform turretRotationPart;
    public Transform firePoint;
    public float turnSpeed = 5f;

    public GameObject[] targetedEnemies = null;
    public string enemyTag = "Enemy";

    private GameObject selectedBuildingPrefab = null;
    //Temporary visual indication for placement
    private GameObject tempIndicator = null; 
    private bool isPlacing = false;
    public LayerMask placementLayer;

    public AudioClip musicClip;
    public AudioSource objectAudioSource;

    void Update()
    {
        if (target != null)
        {
            RotateTurretTowardsTarget();
        }

        if (isPlacing && tempIndicator != null)
        {
            FollowMousePosition();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
    }

    private void RotateTurretTowardsTarget()
    {
        Vector3 direction = target.position - transform.position;

        //Turret rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(turretRotationPart.rotation, targetRotation, Time.deltaTime * turnSpeed).eulerAngles;
        turretRotationPart.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    public void SelectBuilding(int buildingType)
    {
        DestroyTempIndicator();

        switch (buildingType)
        {
            case 1:
                selectedBuildingPrefab = smallPowerBuildingPrefab;
                break;
            case 2:
                selectedBuildingPrefab = mediumPowerBuildingPrefab;
                break;
            case 3:
                selectedBuildingPrefab = largePowerBuildingPrefab;
                break;
            case 4:
                selectedBuildingPrefab = turretPrefab;
                break;
            default:
                Debug.LogWarning("Invalid building type selected.");
                return;
        }

        if (selectedBuildingPrefab == null)
        {
            Debug.LogError("Selected building prefab is null.");
            return;
        }

        //Check if the player can afford the building
        BuildingInfo buildingInfo = selectedBuildingPrefab.GetComponent<BuildingInfo>();
        if (buildingInfo == null)
        {
            Debug.LogError($"Building prefab {selectedBuildingPrefab.name} is missing a BuildingInfo component.");
            return;
        }

        if (GameManager.Instance.CanAfford(buildingInfo.cost))
        {
            GameManager.Instance.DeductCurrency(buildingInfo.cost);

            //Create placement indicator
            tempIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tempIndicator.transform.localScale = selectedBuildingPrefab.transform.localScale * 0.5f;
            tempIndicator.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            //Remove collider from the indicator
            Destroy(tempIndicator.GetComponent<Collider>()); 
            isPlacing = true;
        }
        else
        {
            Debug.LogWarning("Not enough currency to place the building.");
            selectedBuildingPrefab = null;
        }
    }

    private void FollowMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            tempIndicator.transform.position = hit.point;
        }
    }

    private void PlaceBuilding()
    {
        if (tempIndicator == null || selectedBuildingPrefab == null)
        {
            Debug.LogWarning("Cannot place building; either no indicator or no building selected.");
            return;
        }

        Vector3 placementPosition = tempIndicator.transform.position;
        Destroy(tempIndicator);

        Instantiate(selectedBuildingPrefab, placementPosition, Quaternion.identity);
        isPlacing = false;
        selectedBuildingPrefab = null;

        Debug.Log("Building placed successfully.");
    }

    private void DestroyTempIndicator()
    {
        if (tempIndicator != null)
        {
            Destroy(tempIndicator);
            tempIndicator = null;
        }
    }

    public void CancelPlacement()
    {
        DestroyTempIndicator();
        selectedBuildingPrefab = null;
        isPlacing = false;
        Debug.Log("Building placement canceled.");
    }
}
