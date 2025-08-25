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
                Random.Range(minData.baseFireRate, maxData.baseFireRate),
                minData.mode,
                minData.type
            );
        }
    }
}

[System.Serializable]
public struct BasePartData
{
    public weaponType type;
    public fireMode mode;
    public float baseFireRate;

    public BasePartData(float baseFireRate, fireMode mode, weaponType type)
    {
        this.baseFireRate = baseFireRate;
        this.mode = mode;
        this.type = type;
    }
}

public enum weaponType
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

public enum fireMode
{
    single,
    burst,
    auto,
    charge,
}
