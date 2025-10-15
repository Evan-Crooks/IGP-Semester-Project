using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField]
    private List<Projectile> activeProjectiles = new();
    private Queue<Projectile> inactiveProjectiles = new();
    void Update()
    {
        // Move projectiles and expire them if needed, without modifying the list during iteration.
        List<Projectile> toExpire = new();
        foreach (Projectile p in activeProjectiles)
        {
            if (p.gameObject.activeSelf)
            {
                if (p.gameObject && p.gameObject.activeInHierarchy)
                {
                    p.transform.position = p.movementPath(p, Time.deltaTime);
                    p.age += Time.deltaTime;
                    if (p.age >= p.lifetime || (p.range != -1 && Vector2.Distance(p.weaponController.transform.position, p.transform.position) >= p.range))
                    {
                        toExpire.Add(p);
                    }
                }
            }
        }
        foreach (Projectile p in toExpire)
        {
            p.Expire();
        }
    }
    public Projectile Next()
    { //get next inactive projectile and make it active and return it to be initialized
        Projectile nextP;
        if (hasBacklog())
        {
            nextP = inactiveProjectiles.Dequeue();
        }
        //if no inactive projectiles make a new one
        else
        {
            GameObject projectileObj = GenerateProjectileObj();
            nextP = projectileObj.AddComponent<Projectile>();
        }
        activeProjectiles.Add(nextP);
        return nextP;
    }
    public void Deactivate(Projectile p)
    {
        activeProjectiles.Remove(p);
        inactiveProjectiles.Enqueue(p);
    }
    public bool hasBacklog()
    {
        return inactiveProjectiles.Count > 0;
    }
    GameObject GenerateProjectileObj()
    {
        GameObject projectileObj = new("Projectile");
        projectileObj.AddComponent<CircleCollider2D>();
        Rigidbody2D rb = projectileObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.useFullKinematicContacts = true;
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        projectileObj.AddComponent<SpriteRenderer>();
        projectileObj.transform.parent = transform;

        return projectileObj;
    }

}