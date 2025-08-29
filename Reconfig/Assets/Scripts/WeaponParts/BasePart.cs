using UnityEngine;

public abstract class BasePart : WeaponPart
{
    public BasePartData properties = default;

    [SerializeField] //min and max data used for random generation
    protected BasePartData minData, //non randomized data goes in minData
        maxData;

    void Awake()
    {
        if (properties.Equals(default(BasePartData)))
        {
            properties = new BasePartData(
                Random.Range(minData.fireRate, maxData.fireRate),
                minData.mode,
                minData.type,
                Random.Range(minData.damage, maxData.damage),
                Random.Range(minData.range, maxData.range),
                Random.Range(minData.recoil, maxData.recoil),
                Random.Range(minData.accuracy, maxData.accuracy),
                minData.reloadType,
                minData.ammoType
            );
        }
    }
}

[System.Serializable]
public struct BasePartData
{
    public WeaponType type;
    public FireMode mode;
    public float fireRate;
    public float damage;
    public float range;
    public float recoil;
    public float accuracy;
    public ReloadType reloadType;
    public AmmoType ammoType;

    public BasePartData(
        float fireRate,
        FireMode mode,
        WeaponType type,
        float damage,
        float range,
        float recoil,
        float accuracy,
        ReloadType reloadType,
        AmmoType ammoType
    )
    {
        this.fireRate = fireRate;
        this.mode = mode;
        this.type = type;
        this.damage = damage;
        this.range = range;
        this.recoil = recoil;
        this.accuracy = accuracy;
        this.reloadType = reloadType;
        this.ammoType = ammoType;
    }
}

public enum WeaponType
{
    pistol,
    rifle,
    shotgun,
    smg,
    lmg,
    sniper,
    launcher,
    melee,
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