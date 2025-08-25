using UnityEngine;

public class BasicMagazine : MagazinePart {
    public override void onHitEnvironment(Projectile p)
    {
        base.onHitEnvironment(p);
        p.Expire();
    }

}
