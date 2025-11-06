using UnityEngine;

//min and max data used for random generation
//non randomized data goes in minData
//randomized data goes in maxData

public abstract class MagazinePart : WeaponPart
{
    public MagazineType magType;
}

public enum MagazineType
{
    standard, //standard
    recharge, // regenerates ammo over time
    infinite, // no ammo limit
    manual, // load one bullet at a time stopping early will not reload the rest of the magazine
}
