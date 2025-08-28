using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

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
    private float projGravity = 0f;
    [SerializeField]
    private float projDrag = 0.1f;
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
    public ProjectileManager pManager;


    [Header("Weapon Parts")]
    public BarrelPart barrel; // Reference to the Barrel scriptable object
    public MagazinePart magazine; // Reference to the Magazine scriptable object
    public StockPart stock; // Reference to the Stock scriptable object
    public BasePart basePart; // Reference to the Base scriptable object
    public GripPart grip; // Reference to the Grip scriptable object

    [Header("Input")]
    public InputAction fireAction; // Input action for firing the weapon

    void Start()
    {
        // fireAction = InputSystem.actions.FindAction("Attack");
        fireAction.Enable();
        if (barrel != null || magazine != null || stock != null || basePart != null || grip != null)
        {
            AssembleWeapon();
        }
        else
        {
            Debug.LogWarning("Weapon parts are not properly assigned. Please assign at least one part to the weapon.");
        }
    }

    void Update()
    {
        // Check if the fire action is triggered
        if (fireAction.WasPressedThisFrame())
        {
            Fire();
        }
    }

    public void OnAttack(){
        Fire();
    }
    private void AssembleWeapon()
    {
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
        // Barrel logic
        if (barrel != null)
        {
            projRange += barrel.properties.rangeModifier; // distance projectile can travel
            projSpread += barrel.properties.spread; // angle projectile spreads out
            projSpeed *= barrel.properties.speedModifier; // usually a negative value (slows projectile)
        }
        // Magazine logic
        if (magazine != null)
        {

        }
        // Stock logic
        if (stock != null)
        {
            projRecoil -= stock.properties.recoilRecoveryModifier;
            // sway += stock.properties.swayModifier; // If sway exists
            // moveStability += stock.properties.moveStabilityModifier; // If moveStability exists
            // staminaHandling += stock.properties.staminaHandlingModifier; // If staminaHandling exists
        }
        // Grip logic
        if (grip != null)
        {
            projRecoil += grip.properties.recoilModifier;
            // adsSpeed += grip.properties.adsSpeedModifier; // If adsSpeed exists
            // aimMoveSpeed += grip.properties.aimMoveSpeedModifier; // If aimMoveSpeed exists
        }
    }

    void Fire()
    {
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
            Paths.StraightPath
        );
        projectile.transform.position = transform.position;
        var sr = projectile.gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = projSprite;
        projectile.direction = transform.right;
        projectile.gameObject.SetActive(true);
    }


}
