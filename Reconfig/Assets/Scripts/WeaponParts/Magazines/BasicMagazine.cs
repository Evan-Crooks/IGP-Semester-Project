using UnityEngine;

public class BasicMagazine : MagazinePart {
    void Awake()
    {
        base.Awake();
        base.properties.movementPath = Paths.StraightPath;
    }
    public override void onHitEnvironment(Projectile p)
    {
        base.onHitEnvironment(p);
        p.Expire();
    }

}
