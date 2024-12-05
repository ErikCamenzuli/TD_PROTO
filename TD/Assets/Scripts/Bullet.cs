using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 0.5f;
    public int bulletDamage = 2;

    public GameObject impactEffects;
    public Vector3 impactEffectsOffset;

    public GameObject targetObject;
    public Transform targetTransform;

    public void Seek(Transform _target)
    {
        targetTransform = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        targetObject = targetTransform.gameObject;

        Vector3 direction = targetTransform.position - transform.position;
        float distanceThisFrame = bulletSpeed * Time.deltaTime;

        if(direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        GameObject effects = Instantiate(impactEffects, GetComponent<OffsetMethods>().GetImpactEffectsPosition(), transform.rotation);
        Destroy(effects, 2f);

        targetObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(this);
    }
}
