using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private WeaponStats weaponStats;

    public ProjectileManager pManager;

    public GameObject weaponObject;

    [Header("Weapon Parts")]
    public BarrelPart barrel; // Reference to the Barrel scriptable object
    public MagazinePart magazine; // Reference to the Magazine scriptable object
    public StockPart stock; // Reference to the Stock scriptable object
    public BasePart basePart; // Reference to the Base scriptable object
    public GripPart grip; // Reference to the Grip scriptable object

    [Header("Input")]
    public InputAction fireAction; // Input action for firing the weapon

    private float timeSinceFire = 0;


    void Start()
    {
        weaponObject = gameObject;
        pManager = GameObject.FindWithTag("Projectile Manager").GetComponent<ProjectileManager>();
        AssembleWeapon();
    }
    void Update()
    {
        timeSinceFire += Time.deltaTime;
    }

    public void OnAttacInput()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure z is 0 for 2D
        Fire((mousePosition - transform.position).normalized);
    }

    public void AssembleWeapon()
    {
        print("assemble weapon");
        //assign each part
        basePart = weaponObject.GetComponent<BasePart>();
        print(weaponObject.GetComponents<WeaponPart>());


        barrel = weaponObject.GetComponent<BarrelPart>();
        magazine = weaponObject.GetComponent<MagazinePart>();
        stock = weaponObject.GetComponent<StockPart>();
        grip = weaponObject.GetComponent<GripPart>();

        print("assemble weapon 2");
        // Base logic
        weaponStats = basePart.properties;
        print($"properties {basePart.properties.fireRate}");
        //Barrel logic
        weaponStats += barrel.properties;
        //magazine  
        weaponStats += magazine.properties;
        //stock
        weaponStats += stock.properties;
        // Grip logic
        weaponStats += grip.properties;
        print("assemble weapon 3");
        weaponStats.projSprite = magazine.properties.projSprite;
    }

    // 1/fire rate = time between shot
    public void Fire(Vector2 direction)
    {
        // TODO fire rate is limited by frame rate should not be in the future.
        //lock projectile from firing before it should be able to 
        if (timeSinceFire < 1 / weaponStats.fireRate) return;
        print("fire");
        timeSinceFire = 0;

        Projectile projectile = pManager.Next();
        projectile.Initialize(
            weaponStats.projSpeed,
            weaponStats.projDamage,
            weaponStats.projLifetime,
            weaponStats.projRange,
            weaponStats.projGravity,
            weaponStats.projSpread,
            weaponStats.projSize,
            weaponStats.projRotation,
            weaponStats.projScale,
            weaponStats.projMass,
            weaponStats.projBounciness,

            weaponStats.projRecoil,
            weaponStats.projSprite,
            this,
            weaponStats.movementPath//change this with weapon parts
        );
        projectile.transform.position = transform.position;
        SpriteRenderer sr = projectile.gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = projectile.sprite;
        // target mouse position

        projectile.direction = direction; 
        // TODO add controller support
        projectile.gameObject.SetActive(true);
    }


}

//struct to hold values for current weapons stats weapon parts should also use it. so that any stat can be added to everything easily.
[System.Serializable]
public struct WeaponStats
{
    public float fireRate;
    public FireMode mode;
    public float accuracy;
    public ReloadType reloadType;
    public AmmoType ammoType;

    public float projSpeed;
    public float projDamage;
    public float projLifetime;
    public float projRange;
    public float projGravity;
    public float projDrag;
    public float projSpread;
    public float projSize;
    public float projRotation;
    public float projScale;
    public float projMass;
    public float projBounciness;
    public float projFireRate;
    public float projRecoil;
    public Sprite projSprite;

    [System.NonSerialized]
    public System.Func<Projectile, float, Vector3> movementPath;

    public WeaponStats(
        float fireRate,
        FireMode mode,
        float accuracy,
        ReloadType reloadType,
        AmmoType ammoType,
        float projSpeed,
        float projDamage,
        float projLifetime,
        float projRange,
        float projGravity,
        float projDrag,
        float projSpread,
        float projSize,
        float projRotation,
        float projScale,
        float projMass,
        float projBounciness,
        float projFireRate,
        float projRecoil,
        Sprite projSprite,
        System.Func<Projectile, float, Vector3> movementPath
    )
    {
        this.fireRate = fireRate;
        this.mode = mode;
        this.accuracy = accuracy;
        this.reloadType = reloadType;
        this.ammoType = ammoType;

        this.projSpeed = projSpeed;
        this.projDamage = projDamage;
        this.projLifetime = projLifetime;
        this.projRange = projRange;
        this.projGravity = projGravity;
        this.projDrag = projDrag;
        this.projSpread = projSpread;
        this.projSize = projSize;
        this.projRotation = projRotation;
        this.projScale = projScale;
        this.projMass = projMass;
        this.projBounciness = projBounciness;
        this.projFireRate = projFireRate;
        this.projRecoil = projRecoil;
        this.projSprite = projSprite;

        this.movementPath = movementPath;
    }
    public static WeaponStats operator +(WeaponStats a, WeaponStats b)
    {
        return new WeaponStats(
            a.fireRate + b.fireRate,
            a.mode, // or choose a rule for these enums
            a.accuracy + b.accuracy,
            a.reloadType,
            a.ammoType,
            a.projSpeed + b.projSpeed,
            a.projDamage + b.projDamage,
            a.projLifetime + b.projLifetime,
            a.projRange + b.projRange,
            a.projGravity + b.projGravity,
            a.projDrag + b.projDrag,
            a.projSpread + b.projSpread,
            a.projSize + b.projSize,
            a.projRotation + b.projRotation,
            a.projScale + b.projScale,
            a.projMass + b.projMass,
            a.projBounciness + b.projBounciness,
            a.projFireRate + b.projFireRate,
            a.projRecoil + b.projRecoil,
            a.projSprite ?? b.projSprite,
            a.movementPath ?? b.movementPath
        );
    }
    public static WeaponStats operator *(WeaponStats a, WeaponStats b)
    {
        return new WeaponStats(
            a.fireRate * b.fireRate,
            a.mode,
            a.accuracy * b.accuracy,
            a.reloadType,
            a.ammoType,
            a.projSpeed * b.projSpeed,
            a.projDamage * b.projDamage,
            a.projLifetime * b.projLifetime,
            a.projRange * b.projRange,
            a.projGravity * b.projGravity,
            a.projDrag * b.projDrag,
            a.projSpread * b.projSpread,
            a.projSize * b.projSize,
            a.projRotation * b.projRotation,
            a.projScale * b.projScale,
            a.projMass * b.projMass,
            a.projBounciness * b.projBounciness,
            a.projFireRate * b.projFireRate,
            a.projRecoil * b.projRecoil,
            a.projSprite ?? b.projSprite,
            a.movementPath ?? b.movementPath
        );
    }
    public static WeaponStats operator *(WeaponStats a, float v)
    {
        a.fireRate *= v;
        a.accuracy *= v;
        a.projSpeed *= v;
        a.projDamage *= v;
        a.projLifetime *= v;
        a.projRange *= v;
        a.projGravity *= v;
        a.projDrag *= v;
        a.projSpread *= v;
        a.projSize *= v;
        a.projRotation *= v;
        a.projScale *= v;
        a.projMass *= v;
        a.projBounciness *= v;
        a.projFireRate *= v;
        a.projRecoil *= v;
        return a;
    }
    public static WeaponStats operator -(WeaponStats a, WeaponStats b)
    {
        return new WeaponStats(
            a.fireRate - b.fireRate,
            a.mode,
            a.accuracy - b.accuracy,
            a.reloadType,
            a.ammoType,
            a.projSpeed - b.projSpeed,
            a.projDamage - b.projDamage,
            a.projLifetime - b.projLifetime,
            a.projRange - b.projRange,
            a.projGravity - b.projGravity,
            a.projDrag - b.projDrag,
            a.projSpread - b.projSpread,
            a.projSize - b.projSize,
            a.projRotation - b.projRotation,
            a.projScale - b.projScale,
            a.projMass - b.projMass,
            a.projBounciness - b.projBounciness,
            a.projFireRate - b.projFireRate,
            a.projRecoil - b.projRecoil,
            a.projSprite ?? b.projSprite,
            a.movementPath ?? b.movementPath
        );
    }
}
