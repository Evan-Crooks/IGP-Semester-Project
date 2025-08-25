using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
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

    private Func<Projectile, float, Vector2> movementPath;

    public Queue<Projectile> projectilePool = new();


    private ProjectileManager _projectileManager;
    public ProjectileManager ProjectileManager => _projectileManager;


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
        _projectileManager = FindAnyObjectByType<ProjectileManager>();
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

    private void AssembleWeapon()
    {
        // Base logic
        if (basePart != null)
        {
            projFireRate += basePart.properties.baseFireRate;
            // Add more BasePartData properties as needed
            // Example:
            // reloadType = basePart.properties.reloadType;
            // ammoType = basePart.properties.ammoType;
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
        Projectile projectile;
        if (projectilePool.Count > 0)
        {
            projectile = projectilePool.Dequeue();
            if (projectile == null || projectile.gameObject == null) projectile = GenerateProjectile();
        }
        else
        {
            projectile = GenerateProjectile();
        }
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
            this
        );
        projectile.transform.position = transform.position;
        projectile.movementPath = Paths.StraightPath; // Set the movement path function
        projectile.gameObject.GetComponent<SpriteRenderer>().sprite = projSprite;

        projectile.direction = transform.right; // Assuming the weapon's forward direction is up
        projectile.gameObject.SetActive(true);
    }

    Projectile GenerateProjectile()
    {
        GameObject projectileObj = new("Projectile");
        projectileObj.AddComponent<CircleCollider2D>();
        Rigidbody2D rb = projectileObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.useFullKinematicContacts = true;
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        projectileObj.AddComponent<SpriteRenderer>();
        Projectile projectile = projectileObj.AddComponent<Projectile>();
        projectile.gameObject.SetActive(false);
        ProjectileManager.AddProjectile(projectile);
        return projectile;
    }
}
