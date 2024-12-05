using UnityEngine;
using System.Linq;
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
    private GameObject tempIndicator = null; //Temporary visual indication for placement
    private bool isPlacing = false;
    public LayerMask placementLayer;

    public AudioClip musicClip;
    public AudioSource objectAutioSource;

    //private void Start()
    //{
    //    InvokeRepeating("UpdatingTarget", 0f, 0.5f);
    //
    //    ///audio - ADD LATER OR TEMP AUDIO FOR TESTING
    //    //objectAutioSource.clip = musicClip;
    //
    //    GameObject _turretEffect = Instantiate(turretPlacementEffect, GetPlacementPosition(), Quaternion.identity);
    //    if (turretPlacementEffect || _turretEffect == null)
    //    {
    //        return;
    //    }
    //}

    public Vector3 GetPlacementPosition()
    {
        return transform.position + effectsPosition;
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = transform.position - target.position;

        //turret rotation
        Quaternion rotate = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(turretRotationPart.rotation, rotate, Time.deltaTime * turnSpeed).eulerAngles;
        turretRotationPart.rotation = Quaternion.Euler(0, rotation.y, 0);

        if (isPlacing && tempIndicator != null)
        {
            FollowMousePosition();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
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
                Debug.Log("Invalid building type selected.");
                return;
        }

        BuildingInfo buildingInfo = selectedBuildingPrefab.GetComponent<BuildingInfo>();
        if (buildingInfo == null)
        {
            Debug.LogError("BuildingInfo component missing.");
            return;
        }

        if (GameManager.Instance.CanAfford(buildingInfo.cost))
        {
            GameManager.Instance.DeductCurrency(buildingInfo.cost);
            tempIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tempIndicator.transform.localScale = selectedBuildingPrefab.transform.localScale * 0.5f;
            tempIndicator.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            Destroy(tempIndicator.GetComponent<Collider>());
            isPlacing = true;
        }
        else
        {
            Debug.LogWarning("Not enough currency.");
            selectedBuildingPrefab = null;
        }
    }

    private void FollowMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            if (tempIndicator != null)
            {
                tempIndicator.transform.position = hit.point;
            }
        }
    }

    private void PlaceBuilding()
    {
        if (tempIndicator != null)
        {
            Vector3 placementPosition = tempIndicator.transform.position;
            Destroy(tempIndicator);
            Instantiate(selectedBuildingPrefab, placementPosition, Quaternion.identity);
            isPlacing = false;
            selectedBuildingPrefab = null;
            Debug.Log("Building placed successfully.");
        }
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
