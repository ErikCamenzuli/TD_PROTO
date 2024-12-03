using UnityEngine;

public class BuildingSellHandler : MonoBehaviour
{
    public float sellValuePercentage = 0.5f; // Percentage of original cost returned when selling the building

    public void SellBuilding()
    {
        BuildingInfo buildingInfo = GetComponent<BuildingInfo>();
        if (buildingInfo != null)
        {
            // Calculate the amount to refund to the player
            float sellValue = buildingInfo.cost * sellValuePercentage;
            GameManager.Instance.playerCurrency += sellValue;
            Debug.Log("Building sold. Refund amount: " + sellValue);

            // Remove the building's power contribution from GameManager
            if (buildingInfo.powerContribution > 0)
            {
                GameManager.Instance.UnregisterPowerSource(buildingInfo.powerContribution);
                Debug.Log("Power source unregistered for sold building. Power contribution removed: " + buildingInfo.powerContribution);
            }

            // Destroy the building
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("BuildingInfo component missing on this building.");
        }
    }
}
