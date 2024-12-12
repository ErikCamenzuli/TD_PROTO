using UnityEngine;

public class OffsetMethods : MonoBehaviour
{
    private static Vector3 impactEffectsOffsetMethod;
    private static Vector3 turretOffsetMethod;


    private void Start()
    {
        //turretOffsetMethod = GetComponent<BuildingPlacer>().turretOffsetPosition;
        impactEffectsOffsetMethod = GetComponent<Bullet>().impactEffectsOffset;
    }

    public Vector3 GetImpactEffectsPosition()
    {
        return transform.position + impactEffectsOffsetMethod;
    }

    public Vector3 GetPlacementPos()
    {
        return transform.position + turretOffsetMethod;
    }
}
