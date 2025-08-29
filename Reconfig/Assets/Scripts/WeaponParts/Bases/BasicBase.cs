using UnityEngine;

public class BasicBase : BasePart
{
    public override void onHitEntity(Projectile p)
    {
        base.onHitEntity(p);
        p.Expire();
    }
}
