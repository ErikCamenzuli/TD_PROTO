using UnityEngine;

public class PowerBuilding : MonoBehaviour
{
    public float powerContribution = 25f;

    private void OnEnable()
    {
        //Register this building's power contribution with the GameManager
        GameManager.Instance.RegisterPowerSource(powerContribution);
        Debug.Log("Power building registered with power contribution: " + powerContribution);
    }

    private void OnDisable()
    {
        //Unregister this building's power contribution when it’s removed or disabled
        GameManager.Instance.UnregisterPowerSource(powerContribution);
        Debug.Log("Power building unregistered.");
    }
}
