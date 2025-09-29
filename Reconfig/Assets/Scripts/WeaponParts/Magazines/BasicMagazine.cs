using UnityEngine;

public class BasicMagazine : MagazinePart {
    public override void onHitEnvironment(Projectile p)
    {
        base.onHitEnvironment(p);
        p.Expire();
    }
    public override void onHitEntity(Projectile p)
    {
        base.onHitEntity(p);
        p.Expire();
    }
}
