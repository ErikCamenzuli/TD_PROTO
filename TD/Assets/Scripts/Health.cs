using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    private float currentHealth;
    public float fullHealth;
    bool isDead = false;

    public Spawner spawner;

    private void Awake()
    {
        currentHealth = fullHealth;
    }


    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();    
    }

    public void TakeDamage(float damageTaken)
    {
        if(isDead == false)
        {
            NpcDead();
            isDead = true; 
        }
        //End goal for the AI to get to
        if (this.gameObject.tag == /*PUT SOMETHING HERE EVENTUALLY ->*/"")
        {
            SceneManager.LoadScene("");
            return;
        }
    }

    public void NpcDead()
    {
        spawner.enemiesPerwave--;
        Destroy(this);
    }
}
