using UnityEngine;

public abstract class WeaponPart : MonoBehaviour
{
    public Sprite sprite;

    public virtual void onHitEntity(Projectile p) { }

    public virtual void onHitEnvironment(Projectile p) { }

    public virtual void onFireEffect(Projectile p) { }

    public virtual void onExpireEffect(Projectile p) { }

    public virtual void onTravelEffect(Projectile p) { }

    public virtual void moveProjectile(Projectile p) { }
}
