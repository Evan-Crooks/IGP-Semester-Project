using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMagazine : MagazinePart
{
    //basic mag except it has the gravity path
    new void Awake()
    {
        base.Awake();
        properties.movementPath = Paths.GravityPath;
    }
    public override void onHitEnvironment(Projectile p)
    {
        base.onHitEnvironment(p);
        p.Expire();
    }

}

