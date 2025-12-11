using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    private bool isGrappling = false;
    [SerializeField] Grapple grapple;
    Vector2 grappleLocation;
    [SerializeField] float pullSpeed = 8f;
    Rigidbody2D rb;
    [Header("Rope")]
    [SerializeField] Sprite ropeSegmentSprite;
    [SerializeField] float ropeSegmentLength = 0.25f;
    [SerializeField] int ropeSortingOrder = -1;

    // Simple pool of rope links so we can re-use them each frame
    readonly System.Collections.Generic.List<Transform> ropeSegments = new System.Collections.Generic.List<Transform>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void onGrappleInput()
    {
        print(grapple);
        if (!isGrappling) launchGrapple();
        else if (isGrappling) releaseGrapple();
    }

    public void onReleaseGrapple()
    {
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

        // Draw rope immediately toward the moving grapple
        UpdateRopeVisual(transform.position, grapple.transform.position);
    }

    public void grappleResult(bool res)
    {
        if (res)
        {
            grappleLocation = grapple.gameObject.transform.position;
            isGrappling = true;
        }
        else
        {
            isGrappling = false;
            ClearRope();
        }
    }

    void releaseGrapple()
    {
        isGrappling = false;
        grapple.gameObject.SetActive(false);
        grappleLocation = Vector2.zero;
        ClearRope();
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

        // Keep rope visuals updated even while the hook is traveling
        if (isGrappling && grapple.gameObject.activeSelf)
        {
            UpdateRopeVisual(transform.position, grapple.transform.position);
        }
    }

    void UpdateRopeVisual(Vector2 start, Vector2 end)
    {
        if (ropeSegmentSprite == null)
            return;

        float distance = Vector2.Distance(start, end);
        if (distance <= 0.001f)
        {
            ClearRope();
            return;
        }

        // Decide how many segments we need based on the sprite size or a fallback length
        float segmentLen = ropeSegmentLength > 0f ? ropeSegmentLength : ropeSegmentSprite.bounds.size.x;
        int needed = Mathf.Max(1, Mathf.CeilToInt(distance / segmentLen));
        EnsureRopeSegments(needed);

        Vector2 dir = (end - start).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        float offset = segmentLen * 0.5f;

        for (int i = 0; i < needed; i++)
        {
            Transform seg = ropeSegments[i];
            seg.gameObject.SetActive(true);
            Vector2 pos = start + dir * (i * segmentLen + offset);
            seg.position = pos;
            seg.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Disable any extras
        for (int i = needed; i < ropeSegments.Count; i++)
        {
            ropeSegments[i].gameObject.SetActive(false);
        }
    }

    void EnsureRopeSegments(int needed)
    {
        while (ropeSegments.Count < needed)
        {
            GameObject link = new GameObject("RopeSegment");
            link.transform.SetParent(transform, true);
            var sr = link.AddComponent<SpriteRenderer>();
            sr.sprite = ropeSegmentSprite;
            sr.sortingOrder = ropeSortingOrder;
            ropeSegments.Add(link.transform);
        }
    }

    void ClearRope()
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            ropeSegments[i].gameObject.SetActive(false);
        }
    }
}
