using UnityEngine;

public class Grapple : MonoBehaviour
{
    Vector2 direction;
    float ttd;
    float speed;
    GrappleHook owner;
    bool active = false;
    Rigidbody2D rb;
    Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void Launch(GrappleHook owner, Vector2 direction, float timeToDeath, float speed)
    {
        // Detach so it no longer follows the player hierarchy
        transform.SetParent(null, true);
        this.owner = owner;
        this.direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector2.zero;
        ttd = timeToDeath;
        this.speed = speed;
        active = true;

        if (this.direction != Vector2.zero)
        {
            // Sprite faces up at 0 deg; subtract 90 to align with direction (so right points right)
            float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Ignore collision with the owner so only terrain stops the grapple
        if (col != null && owner != null)
        {
            Collider2D ownerCol = owner.GetComponent<Collider2D>();
            if (ownerCol != null)
            {
                Physics2D.IgnoreCollision(col, ownerCol, true);
            }
        }
    }

    void Update()
    {
        if (active)
        {
            if (ttd <= 0f || direction == Vector2.zero)
                return;

            ttd -= Time.deltaTime;

            if (ttd <= 0f)
            {
                if (rb != null) rb.velocity = Vector2.zero;
                owner.grappleResult(false);
                gameObject.SetActive(false);
                active = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!active || direction == Vector2.zero)
            return;

        Vector2 newPos = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
        if (rb != null)
        {
            rb.MovePosition(newPos);
        }
        else
        {
            transform.position = newPos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print($"grapple hit {other.gameObject.name}");
        // Terrain detection by tag or a specific layer (e.g., layer 3)
        if (other.gameObject.CompareTag("Terrain") || other.gameObject.layer == 3)
        {
            owner.grappleResult(true);
            direction = Vector2.zero;
            active = false;
        }
    }
}
