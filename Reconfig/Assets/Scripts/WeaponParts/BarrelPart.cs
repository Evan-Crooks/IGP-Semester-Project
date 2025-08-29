using Unity.VisualScripting;
using UnityEngine;

public abstract class BarrelPart : WeaponPart
{
    //min and max data used for random generation
    //non randomized data goes in minData
    //randomized data goes in maxData

    public BarrelPartData properties = default;

    [SerializeField]
    protected BarrelPartData minData,
        maxData;

    void Awake()
    {
        // Only initialize if properties is default (all fields are zero)
        if (properties.Equals(default(BarrelPartData)))
        {
            properties = new BarrelPartData(
                Random.Range(minData.rangeModifier, maxData.rangeModifier),
                Random.Range(minData.accuracyModifier, maxData.accuracyModifier),
                Random.Range(minData.spread, maxData.spread),
                Random.Range(minData.speedModifier, maxData.speedModifier)
            );
        }
    }
}

[System.Serializable]
public struct BarrelPartData
{
    public float rangeModifier;
    public float accuracyModifier;
    public float spread;
    public float speedModifier; // Added speed mod

    public BarrelPartData(
        float rangeModifier,
        float accuracyModifier,
        float spread,
        float speedModifier // Added parameter
    )
    {
        this.rangeModifier = rangeModifier;
        this.accuracyModifier = accuracyModifier;
        this.spread = spread;
        this.speedModifier = speedModifier; // Assign value
    }
}
