using UnityEngine;

//min and max data used for random generation
//non randomized data goes in minData
//randomized data goes in maxData

public abstract class MagazinePart : WeaponPart
{
    public MagazinePartData properties = default;

    public Sprite projectileSprite;

    [SerializeField]
    protected MagazinePartData minData,
        maxData;

    void Awake()
    {
        if (properties.Equals(default(MagazinePartData)))
        {
            properties = new MagazinePartData(
                Random.Range(minData.ammoCapacity, maxData.ammoCapacity),
                Random.Range(minData.reloadTime, maxData.reloadTime),
                minData.magazineType
            );
        }
    }
}

[System.Serializable]
public struct MagazinePartData
{
    public int ammoCapacity;
    public float reloadTime;
    public MagazineType magazineType;

    public MagazinePartData(int ammoCapacity, float reloadTime, MagazineType magazineType)
    {
        this.ammoCapacity = ammoCapacity;
        this.reloadTime = reloadTime;
        this.magazineType = magazineType;
    }
}

public enum MagazineType
{
    standard, //standard
    recharge, // regenerates ammo over time
    infinite, // no ammo limit
    manual, // load one bullet at a time stopping early will not reload the rest of the magazine
}
