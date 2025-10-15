using UnityEngine;

public abstract class GripPart : WeaponPart
{
    //min and max data used for random generation
    //non randomized data goes in minData
    //randomized data goes in maxData

    public GripPartData properties = default;

    [SerializeField]
    protected GripPartData minData,
        maxData;

    void Awake()
    {
        if (properties.Equals(default(GripPartData)))
        {
            properties = new GripPartData(
                Random.Range(minData.recoilModifier, maxData.recoilModifier),
                Random.Range(minData.adsSpeedModifier, maxData.adsSpeedModifier),
                Random.Range(minData.aimMoveSpeedModifier, maxData.aimMoveSpeedModifier)
            );
        }
    }
}

[System.Serializable]
public struct GripPartData
{
    public float recoilModifier;
    public float adsSpeedModifier;
    public float aimMoveSpeedModifier;

    public GripPartData(float recoilModifier, float adsSpeedModifier, float aimMoveSpeedModifier)
    {
        this.recoilModifier = recoilModifier;
        this.adsSpeedModifier = adsSpeedModifier;
        this.aimMoveSpeedModifier = aimMoveSpeedModifier;
    }
}
