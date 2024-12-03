using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public int cost;                   // Cost of the building
    public float powerContribution = 0; // Amount of power this building generates (0 if it doesn't generate power)
    public float powerRequirement = 0;  // Amount of power required to operate (set for turrets)

    // Method to calculate and return 50% of the building's value
    public int GetResaleValue()
    {
        return Mathf.RoundToInt(cost * 0.5f);
    }
}
