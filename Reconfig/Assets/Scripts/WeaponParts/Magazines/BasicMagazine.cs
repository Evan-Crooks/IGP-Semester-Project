using UnityEngine;

public class BasicMagazine : MagazinePart {
    new void Awake()
    {
        // Use the included knob sprite as the projectile sprite.
        minCoeff.projSprite = Resources.Load<Sprite>("basic bullet");
        base.Awake();
        base.properties.movementPath = Paths.StraightPath;
    }
    public override void onHitEnvironment(Projectile p)
    {
        base.onHitEnvironment(p);
        p.Expire();
    }

}
