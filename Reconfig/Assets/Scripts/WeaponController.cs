using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private FireMode mode;
    [SerializeField] private WeaponType type;
    [SerializeField] private float accuracy;
    [SerializeField] ReloadType reloadType;
    [SerializeField] AmmoType ammoType;

    [SerializeField]
    private float projSpeed = 10f;
    [SerializeField]
    private float projDamage = 5f;
    [SerializeField]
    private float projLifetime = 2f;
    [SerializeField]
    private float projRange = 20f;
    [SerializeField]
    private float projGravity = 1f;
    [SerializeField]
    private float projDrag = 0.1f; //unused rn
    [SerializeField]
    private float projSpread = 0.05f;
    [SerializeField]
    private float projSize = 1f;
    [SerializeField]
    private float projRotation = 0f;
    [SerializeField]
    private float projScale = 1f;
    [SerializeField]
    private float projMass = 1f;
    [SerializeField]
    private float projBounciness = 0.2f;
    [SerializeField]
    private float projFireRate = 0.5f;
    [SerializeField]
    private float projRecoil = 1f;
    [SerializeField]
    private Sprite projSprite = null; // Placeholder for the sprite, can be set later
    private System.Func<Projectile, float, Vector3> movementPath = Paths.StraightPath;

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
        weaponObject = GameObject.Find("Player Weapon");
        if (weaponObject == null) print("player needs gameobject called \"Player Weapon\" to function, \r it should have basic part scripts assigned by default but can be switched.");
        AssembleWeapon();
    }
    void Update()
    {
        timeSinceFire += Time.deltaTime;
    }

    public void OnAttacInput()
    {
        Fire();
    }
    public void AssembleWeapon()
    {
        //assign each part
        basePart = weaponObject.GetComponent<BasePart>();
        barrel = weaponObject.GetComponent<BarrelPart>();
        magazine = weaponObject.GetComponent<MagazinePart>();
        stock = weaponObject.GetComponent<StockPart>();
        grip = weaponObject.GetComponent<GripPart>();

        // Base logic
        if (basePart != null)
        {
            // Use BasePartData as initial values where applicable
            // Assign base part properties to weapon controller fields
            fireRate = basePart.properties.fireRate;
            projDamage = basePart.properties.damage;
            projRange = basePart.properties.range;
            projRecoil = basePart.properties.recoil;
            mode = basePart.properties.mode;
            type = basePart.properties.type;
            accuracy = basePart.properties.accuracy;
            reloadType = basePart.properties.reloadType;
            ammoType = basePart.properties.ammoType;
        }
        else
        {
            Debug.LogWarning("BasePart not found on weaponObject.");
        }

        // Barrel logic
        if (barrel != null)
        {
            projRange += barrel.properties.rangeModifier; // distance projectile can travel
            projSpread += barrel.properties.spread; // angle projectile spreads out
            projSpeed *= barrel.properties.speedModifier; // usually a negative value (slows projectile)
        }
        else
        {
            Debug.LogWarning("BarrelPart not found on weaponObject.");
        }

        // Magazine logic
        if (magazine != null)
        {
            movementPath = magazine.properties.movementPath;
        }
        else
        {
            Debug.LogWarning("MagazinePart not found on weaponObject.");
        }

        // Stock logic
        if (stock != null)
        {
            projRecoil -= stock.properties.recoilRecoveryModifier;
            // sway += stock.properties.swayModifier; // If sway exists
            // moveStability += stock.properties.moveStabilityModifier; // If moveStability exists
            // staminaHandling += stock.properties.staminaHandlingModifier; // If staminaHandling exists
        }
        else
        {
            Debug.LogWarning("StockPart not found on weaponObject.");
        }

        // Grip logic
        if (grip != null)
        {
            projRecoil += grip.properties.recoilModifier;
            // adsSpeed += grip.properties.adsSpeedModifier; // If adsSpeed exists
            // aimMoveSpeed += grip.properties.aimMoveSpeedModifier; // If aimMoveSpeed exists
        }
        else
        {
            Debug.LogWarning("GripPart not found on weaponObject.");
        }
    }

    // 1/fire rate =
    void Fire()
    {
        //lock projectile from firing before it should be able to 
        if (timeSinceFire < 1 / fireRate) return;
        print("fire");
        timeSinceFire = 0;

        Projectile projectile = pManager.Next();
        projectile.Initialize(
            projSpeed,
            projDamage,
            projLifetime,
            projRange,
            projGravity,
            projSpread,
            projSize,
            projRotation,
            projScale,
            projMass,
            projBounciness,
            projFireRate,
            projRecoil,
            projSprite,
            this,
            movementPath//change this with weapon parts
        );
        projectile.transform.position = transform.position;
        var sr = projectile.gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = projSprite;
        // target mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure z is 0 for 2D
        projectile.direction = (mousePosition - transform.position).normalized;
        // TODO add controller support
        projectile.gameObject.SetActive(true);
    }


}
