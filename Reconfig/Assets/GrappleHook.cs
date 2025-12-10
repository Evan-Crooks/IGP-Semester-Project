using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    private bool isGrappling = false;
    [SerializeField] Grapple grapple;
    Vector2 grappleLocation;
    [SerializeField] float pullSpeed = 8f;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void onGrappleInput()
    {
        print(grapple);
        if (!isGrappling) launchGrapple();
    }

    public void onReleaseGrapple()
    {
        if (isGrappling) releaseGrapple();
    }

    void launchGrapple()
    {
        isGrappling = true;

        // Direction from this object to the mouse (in world space)
        Vector3 mouseScreen = Input.mousePosition;
        // Use the object's depth so ScreenToWorldPoint returns a point on the same plane
        float depth = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, depth));
        Vector3 direction = (mouseWorld - transform.position).normalized;

        grapple.transform.position = transform.position;
        grapple.gameObject.SetActive(true);
        grapple.Launch(this, direction, 1.5f, 10f);
    }

    public void grappleResult(bool res)
    {
        if (res)
        {
            grappleLocation = grapple.gameObject.transform.position;
            isGrappling = true;
        }
        else isGrappling = false;
    }

    void releaseGrapple()
    {
        isGrappling = false;
        grapple.gameObject.SetActive(false);
        grappleLocation = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (isGrappling && grappleLocation != Vector2.zero)
        {
            // Pull toward the latched grapple point
            Vector2 pullDir = (grappleLocation - (Vector2)transform.position).normalized;
            if (rb != null)
            {
                rb.velocity += pullDir * pullSpeed;
            }
            else
            {
                transform.position += (Vector3)(pullDir * pullSpeed * Time.deltaTime);
            }
            if (Vector2.Distance(transform.position, grappleLocation) < 0.5f) releaseGrapple();
        }
    }
}
