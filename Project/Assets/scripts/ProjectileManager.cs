using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    public List<Projectile> projectiles = new();
    void Update()
    {
        //move projectiles if the should expire tell them to.
        foreach (Projectile p in projectiles)
        {
            if (p != null)
            {

                if (p.gameObject && p.gameObject.activeInHierarchy)
                {
                    p.transform.position = p.movementPath(p, Time.deltaTime);
                    p.age += Time.deltaTime;
                    if (p.age >= p.lifetime || (p.range != -1 && Vector2.Distance(p.weaponController.transform.position, p.transform.position) >= p.range))
                    {
                        p.Expire();
                    }
                }
            }
            else RemoveProjectile(p);

        }
    }
    public void AddProjectile(Projectile p)
    {
        projectiles.Add(p);
    }
    private void RemoveProjectile(Projectile p)
    {
        if (projectiles.Contains(p))
        {
            projectiles.Remove(p);
        }
    }
}