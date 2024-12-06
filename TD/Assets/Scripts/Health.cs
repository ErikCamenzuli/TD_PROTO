using UnityEngine;

public class Health : MonoBehaviour
{
    public float fullHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

    private void Awake()
    {
        currentHealth = fullHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            isDead = true;
            OnDeath?.Invoke(); // Trigger the death event
            Destroy(gameObject);
        }
    }
}
