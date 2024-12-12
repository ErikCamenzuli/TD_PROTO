using UnityEngine;

public class Turret : MonoBehaviour
{
    public float powerRequirement = 10f; 
    private bool isActive = false;

    private Transform target;
    public Transform turretRotationPoint;
    public Transform firePoint;

    public GameObject placementEffects;
    public GameObject bulletPrefab;

    public GameObject[] targetedAI = null;
    public GameObject[] groundAI = null;
    public string tagAI = "Enemy"; 

    public float turretRange;
    public float fireRate;
    public float fireCountDown;
    public float turnSpeed = 5f;


    public AudioClip musicClip;
    public AudioSource objectAudioSource;


    private void Start()
    {
        //Check if there is enough power to activate this turret when it's created
        if (GameManager.Instance.HasSufficientPower(powerRequirement))
        {
            ActivateTurret();
        }
        else
        {
            Debug.LogWarning("Not enough power to activate turret.");
        }

        InvokeRepeating("UpdatingTarget", 0f, 0.5f);
        objectAudioSource.clip = musicClip;
        GameObject _turretEffects = Instantiate(placementEffects, GetComponent<OffsetMethods>().GetPlacementPos(), Quaternion.identity);
        if (placementEffects != null)
        {
            return;
        }

    }

    private void ActivateTurret()
    {
        if (!isActive)
        {
            isActive = true;
            GameManager.Instance.AddTurretPowerRequirement(powerRequirement);
            Debug.Log("Turret activated with power requirement: " + powerRequirement);
        }
    }
    private void Update()
    {
        //Continuously check if power is sufficient and toggle activation accordingly
        if (isActive && !GameManager.Instance.HasSufficientPower(powerRequirement))
        {
            DeactivateTurret();
        }
        else if (!isActive && GameManager.Instance.HasSufficientPower(powerRequirement))
        {
            ActivateTurret();
        }

        if (target ==null)
        {
            return;
        }

        Vector3 targetDirection = target.position - transform.position;
        Quaternion turretRotate = Quaternion.LookRotation(targetDirection);
        Vector3 rotation = Quaternion.Lerp(turretRotationPoint.rotation, turretRotate, Time.deltaTime * turnSpeed).eulerAngles;
        turretRotationPoint.rotation = Quaternion.Euler(0, rotation.y, 0);

        if(fireCountDown <=0f)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    public void Shoot()
    {
        GameObject bulletShot = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletComponent = bulletShot.GetComponent<Bullet>();

        if (bulletComponent != null)
        {
            objectAudioSource.Play();
            bulletComponent.Seek(target);
        }
    }

    void UpdatingTarget()
    {
        groundAI = GameObject.FindGameObjectsWithTag(tagAI);

        if(gameObject.tag == "Turret")
        {
            targetedAI = groundAI;
        }

        float shortDistanceCheck = Mathf.Infinity;
        GameObject nearestAI = null;

        foreach (GameObject enemyAI in targetedAI)
        {
            float distanceToAI = Vector3.Distance(transform.position, enemyAI.transform.position);

            if(distanceToAI < shortDistanceCheck)
            {
                shortDistanceCheck = distanceToAI;
                nearestAI = enemyAI;
            }
        }

        if (nearestAI != null && shortDistanceCheck <= turretRange) 
        {
            target = nearestAI.transform;
        }
        else
        {
            target = null;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, turretRange);
    }

    private void DeactivateTurret()
    {
        if (isActive)
        {
            isActive = false;
            GameManager.Instance.RemoveTurretPowerRequirement(powerRequirement);
            Debug.Log("Turret deactivated due to insufficient power.");
        }
    }


    private void OnDestroy()
    {
        //Ensure power requirement is removed when the turret is destroyed
        if (isActive)
        {
            GameManager.Instance.RemoveTurretPowerRequirement(powerRequirement);
        }
    }
}
