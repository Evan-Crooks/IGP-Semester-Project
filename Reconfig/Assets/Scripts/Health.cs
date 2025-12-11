using UnityEngine;


class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;

    private int health;

    [SerializeField]
    private int armor;
    [SerializeField] HealthBar healthBar;
    private void Awake()
    {
        //set health to max health
        health = maxHealth;
        //get health bar component
        healthBar = GetComponentInChildren<HealthBar>();
        //update health bar to be max value
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void DealDamage(int damage, int armorPen)
    {
        // subject to change: total damage = baseDamage - max((armor + ArmorPen), 0)
        health -= damage - ((armor - armorPen) < 0 ? 0 : armor - armorPen);
        //update health bar
        healthBar.UpdateHealthBar(health, maxHealth);
        Debug.Log("Enemy Health: " + health);
        // if health is zero
        if(health < 0)
        {
            //emit some kind of score event if object is an enemy

            //destroy entity
            Destroy(gameObject);
        }
    }
}