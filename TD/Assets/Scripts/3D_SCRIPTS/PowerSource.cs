using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public float powerContribution = 5f;

    private void OnEnable()
    {
        //Register this building's power contribution with the GameManager
        GameManager.Instance.RegisterPowerSource(powerContribution);
    }

    private void OnDisable()
    {
        //Unregister this building's power contribution when it’s removed or disabled
        GameManager.Instance.UnregisterPowerSource(powerContribution);
    }
}
