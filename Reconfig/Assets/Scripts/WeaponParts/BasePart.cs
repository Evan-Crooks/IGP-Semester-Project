using UnityEngine;

public abstract class BasePart : WeaponPart
{
    public FireMode mode;
    public ReloadType reloadType;
    public AmmoType ammoType;
}
public enum ReloadType
{
    single,
    full,
    recharge,
}

public enum FireMode
{
    single,
    burst,
    auto,
    charge,
}

public enum AmmoType
{
    projectile,
    beam,
    hitscan,
}