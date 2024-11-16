using UnityEngine;

public class Turret : MonoBehaviour
{
    public float powerRequirement = 10f; // Define the power requirement for this turret

    private bool isActive = false;

    private void Start()
    {
        // Check if there is enough power to activate this turret when it's created
        if (GameManager.Instance.HasSufficientPower(powerRequirement))
        {
            ActivateTurret();
        }
        else
        {
            Debug.LogWarning("Not enough power to activate turret.");
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

    private void DeactivateTurret()
    {
        if (isActive)
        {
            isActive = false;
            GameManager.Instance.RemoveTurretPowerRequirement(powerRequirement);
            Debug.Log("Turret deactivated due to insufficient power.");
        }
    }

    private void Update()
    {
        // Continuously check if power is sufficient and toggle activation accordingly
        if (isActive && !GameManager.Instance.HasSufficientPower(powerRequirement))
        {
            DeactivateTurret();
        }
        else if (!isActive && GameManager.Instance.HasSufficientPower(powerRequirement))
        {
            ActivateTurret();
        }
    }

    private void OnDestroy()
    {
        // Ensure power requirement is removed when the turret is destroyed
        if (isActive)
        {
            GameManager.Instance.RemoveTurretPowerRequirement(powerRequirement);
        }
    }
}
