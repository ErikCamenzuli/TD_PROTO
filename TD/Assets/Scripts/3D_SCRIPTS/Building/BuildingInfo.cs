using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public int cost;                   
    public float powerContribution = 0; 
    public float powerRequirement = 0;  

    //Method to calculate and return 50% of the building's value
    public int GetResaleValue()
    {
        return Mathf.RoundToInt(cost * 0.5f);
    }
}
