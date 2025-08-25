using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Vector2 direction;
    public float damage;
    public float age;
    public float lifetime;
    public float range;
    public float gravity;
    public float spread;
    public float size;
    public float rotation;
    public float scale;
    public float mass;
    public float bounciness;
    public float fireRate;
    public float recoil;
    public Sprite sprite;

    public System.Func<Projectile, float, Vector3> movementPath;

    public WeaponController weaponController;

    public void Initialize(
        float speed,
        float damage,
        float lifetime,
        float range,
        float gravity,
        float spread,
        float size,
        float rotation,
        float scale,
        float mass,
        float bounciness,
        float fireRate,
        float recoil,
        Sprite sprite,
        WeaponController weaponController
    )
    {
        this.speed = speed;
        this.damage = damage;
        this.lifetime = lifetime;
        this.range = range;
        this.gravity = gravity;
        this.spread = spread;
        this.size = size;
        this.rotation = rotation;
        this.scale = scale;
        this.mass = mass;
        this.bounciness = bounciness;
        this.fireRate = fireRate;
        this.recoil = recoil;
        this.sprite = sprite;
        this.weaponController = weaponController;

        age = 0f;

    }

    public void onHitEntity()
    {
        weaponController.basePart.onHitEntity(this);
        weaponController.barrel.onHitEntity(this);
        weaponController.grip.onHitEntity(this);
        weaponController.stock.onHitEntity(this);
        weaponController.magazine.onHitEntity(this);
    }

    public void onHitEnvironment()
    {
        weaponController.basePart.onHitEnvironment(this);
        weaponController.barrel.onHitEnvironment(this);
        weaponController.grip.onHitEnvironment(this);
        weaponController.stock.onHitEnvironment(this);
        weaponController.magazine.onHitEnvironment(this);
    }

    public void onTravel()
    {
        weaponController.basePart.onTravelEffect(this);
        weaponController.barrel.onTravelEffect(this);
        weaponController.grip.onTravelEffect(this);
        weaponController.stock.onTravelEffect(this);
        weaponController.magazine.onTravelEffect(this);
    }

    public void onExpire()
    {
        weaponController.basePart.onExpireEffect(this);
        weaponController.barrel.onExpireEffect(this);
        weaponController.grip.onExpireEffect(this);
        weaponController.stock.onExpireEffect(this);
        weaponController.magazine.onExpireEffect(this);
    }

    public void Expire()
    {
        onExpire();
        Reset();
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        weaponController.projectilePool.Enqueue(this);

        speed = 0f;
        direction = Vector2.zero;
        damage = 0f;
        age = 0f;
        lifetime = 0f;
        range = 0f;
        gravity = 0f;
        spread = 0f;
        size = 0f;
        rotation = 0f;
        scale = 0f;
        mass = 0f;
        bounciness = 0f;
        fireRate = 0f;
        recoil = 0f;
        sprite = null;
        movementPath = null;
        weaponController = null;


    }
    
    void OnCollisionEnter2D(Collision2D other) {
        print(other.gameObject.tag);
        if (other.gameObject.CompareTag("Terrain")) { onHitEnvironment(); } 
    }
}
