using UnityEngine;

public abstract class StockPart : WeaponPart
{
    //min and max data used for random generation
    //non randomized data goes in minData
    //randomized data goes in maxData

    public StockPartData properties = default;

    [SerializeField]
    protected StockPartData minData,
        maxData;

    void Awake()
    {
        if (properties.Equals(default(StockPartData)))
        {
            properties = new StockPartData(
                Random.Range(minData.recoilRecoveryModifier, maxData.recoilRecoveryModifier),
                Random.Range(minData.swayModifier, maxData.swayModifier),
                Random.Range(minData.moveStabilityModifier, maxData.moveStabilityModifier),
                Random.Range(minData.staminaHandlingModifier, maxData.staminaHandlingModifier)
            );
        }
    }
}

[System.Serializable]
public struct StockPartData
{
    public float recoilRecoveryModifier;
    public float swayModifier;
    public float moveStabilityModifier;
    public float staminaHandlingModifier;

    public StockPartData(
        float recoilRecoveryModifier,
        float swayModifier,
        float moveStabilityModifier,
        float staminaHandlingModifier
    )
    {
        this.recoilRecoveryModifier = recoilRecoveryModifier;
        this.swayModifier = swayModifier;
        this.moveStabilityModifier = moveStabilityModifier;
        this.staminaHandlingModifier = staminaHandlingModifier;
    }
}
