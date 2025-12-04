using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* TODO: Upgrade so that we have a component called damage*/
        // Will upgrade such that other things can be damaged in the future
        if (collision.gameObject.tag == "Player")
        {
            // Damage the entity and do knockback if applicable
            Vector2 direction = (collision.gameObject.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, direction);
        }
    }
}
