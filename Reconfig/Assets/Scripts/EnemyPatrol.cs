using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    public float speed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;

    }

    Vector2 direction = Vector2.right;
    // Update is called once per frame
    void Update()
    {
        //points can be any height with this
        if(!IsBetween(gameObject.transform.position, pointA.transform.position, pointB.transform.position)) 
            direction= Vector2.right * PatrolDirection(gameObject.transform.position, pointA.transform.position, pointB.transform.position);
        rb.velocity = direction * speed;
    }

    //check if point is between 2 other points
    bool IsBetween(Vector2 current, Vector2 a, Vector2 b)
    {
        float minX = Mathf.Min(a.x, b.x);
        float maxX = Mathf.Max(a.x, b.x);
        if (Mathf.Approximately(minX, maxX)) return Mathf.Approximately(current.x, minX);
        return current.x >= minX && current.x <= maxX;
    }

    //return direction to go to.
    int PatrolDirection(Vector2 current, Vector2 leftPoint, Vector2 rightPoint)
    {
        // Compare only the X offsets to decide which way to move.
        float offsetLeft = leftPoint.x - current.x;
        float offsetRight = rightPoint.x - current.x;

        float targetOffset = Mathf.Abs(offsetLeft) >= Mathf.Abs(offsetRight) ? offsetLeft : offsetRight;
        float dir = Mathf.Sign(targetOffset);
        return dir == 0f ? 1 : (int)dir; // default to right if exactly aligned
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.2f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.2f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
