using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WeaponPart : MonoBehaviour
{
    public Sprite sprite;

    public RarityLevel rarity;
    public WeaponStats minCoeff, maxCoeff, properties;


    protected void Awake()
    {
        if (rarity == 0)
        {
            float rand = Random.Range(0f, 1f);
            if (rand < 0.5f)
                rarity = RarityLevel.Common;
            else if (rand < 0.75f)
                rarity = RarityLevel.Uncommon;
            else if (rand < 0.9f)
                rarity = RarityLevel.Rare;
            else if (rand < 0.98f)
                rarity = RarityLevel.Epic;
            else
                rarity = RarityLevel.Legendary;
        }
        // Initialize randomized properties only if not already assigned (e.g., when not loaded from save)
        if (properties.Equals(default(WeaponStats)))
            randomizeStats();
    }

    public void randomizeStats()
    {
        int rarityFactor = (int)rarity;
        if (rarityFactor <= 0) rarityFactor = 1;

        // Make tiers contiguous: each tier starts at the previous tier's max and has the same width (maxCoeff - minCoeff)
        WeaponStats width = maxCoeff - minCoeff;
        WeaponStats min = minCoeff + width * (rarityFactor - 1);
        WeaponStats max = min + width;

        // Enums take minvalue, 
        properties.mode = min.mode;
        properties.reloadType = min.reloadType;
        properties.ammoType = min.ammoType;

        // Numeric fields randomized between min and max
        properties.fireRate = Random.Range(min.fireRate, max.fireRate);
        properties.accuracy = Random.Range(min.accuracy, max.accuracy);

        properties.projSpeed = Random.Range(min.projSpeed, max.projSpeed);
        properties.projDamage = Random.Range(min.projDamage, max.projDamage);
        properties.projLifetime = Random.Range(min.projLifetime, max.projLifetime);
        properties.projRange = Random.Range(min.projRange, max.projRange);
        properties.projGravity = Random.Range(min.projGravity, max.projGravity);
        properties.projDrag = Random.Range(min.projDrag, max.projDrag);
        properties.projSpread = Random.Range(min.projSpread, max.projSpread);
        properties.projSize = Random.Range(min.projSize, max.projSize);
        properties.projRotation = Random.Range(min.projRotation, max.projRotation);
        properties.projScale = Random.Range(min.projScale, max.projScale);
        properties.projMass = Random.Range(min.projMass, max.projMass);
        properties.projBounciness = Random.Range(min.projBounciness, max.projBounciness);
        properties.projFireRate = Random.Range(min.projFireRate, max.projFireRate);
        properties.projRecoil = Random.Range(min.projRecoil, max.projRecoil);

        // others use min if min not set use max.
        properties.projSprite = min.projSprite ?? max.projSprite;
        properties.movementPath = min.movementPath ?? max.movementPath;
    }

    public virtual void onHitEntity(Projectile p) { }

    public virtual void onHitEnvironment(Projectile p) { }

    public virtual void onFireEffect(Projectile p) { }

    public virtual void onExpireEffect(Projectile p) { }

    public virtual void onTravelEffect(Projectile p) { }

    public virtual void moveProjectile(Projectile p) { }
}

public enum RarityLevel
{
    None,      // 0
    Common,    // 1
    Uncommon,  // 2
    Rare,      // 3
    Epic,      // 4
    Legendary  // 5
}
