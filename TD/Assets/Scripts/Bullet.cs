using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 0.5f;
    public int bulletDamage = 2;

    public GameObject impactEffects;
    public Vector3 impactEffectsOffset;

    private Transform targetTransform;

    public void Seek(Transform _target)
    {
        targetTransform = _target;
    }

    void Update()
    {
        if (targetTransform == null)
        {
            Destroy(gameObject); 
            return;
        }

        Vector3 direction = targetTransform.position - transform.position;
        float distanceThisFrame = bulletSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (impactEffects != null)
        {
            //Instantiate impact effects at the correct position and orientation
            Vector3 impactPosition = transform.position + impactEffectsOffset;
            GameObject effects = Instantiate(impactEffects, impactPosition, Quaternion.identity);
            Destroy(effects, 2f); 
        }

        if (targetTransform != null)
        {
            //Check if the target has a Health component and deal damage
            Health targetHealth = targetTransform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(bulletDamage);
            }
            else
            {
                Debug.LogWarning("Target does not have a Health component.");
            }
        }

        Destroy(gameObject); 
    }
}
