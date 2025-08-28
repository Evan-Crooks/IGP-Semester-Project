using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{

    public UnityEvent OnHealthChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int health;
    public int maxHealth = 10;

    private KnockBack knockback;
    void Start()
    {
        health = maxHealth;
        knockback = GetComponent<KnockBack>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        // Update health
        health -= amount;

        // Send a signal so ui updates
        OnHealthChanged.Invoke(); //send out a signal saying health changed

        // Handle knockback
        if (knockback != null)
        {
            knockback.callKnockback(hitDirection, Vector2.zero, 0);
        }

        // Die if needed
        if (health <= 0)
        {
            //make sure character dies
            Destroy(gameObject, 0.01f);
        }
    }
}
