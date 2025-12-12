using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementAI : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float pathUpdateRate = 0.5f;
    public float stopDistance = 0.5f;
    public float slowDownDistance = 0.2f;

    private Transform player;
    private List<GridNode> currentPath;
    private int pathIndex = 0;
    private float pathTimer = 0f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }
        //update timer and potentially update path
        pathTimer += Time.deltaTime;
        if(pathTimer > pathUpdateRate)
        {
            //reset timer
            pathTimer = 0f;
            //get new path to player
            if(Vector3.Distance(transform.position, player.transform.position) > stopDistance)
            {
                if(AStar.instance == null)
                {
                    return;
                }
                Vector2Int start = AStar.instance.WorldToGrid(transform.position);
                Vector2Int end = AStar.instance.WorldToGrid(player.transform.position);
                pathIndex = 1;
                currentPath = AStar.instance.FindPath(start, end);
            }
        }
        //move enemy along path
        if(currentPath == null || currentPath.Count == 0 || pathIndex >= currentPath.Count)
        {
            return;
        }
        Vector3 targetPos = new Vector3(
            currentPath[pathIndex].WorldPosition.x,
            currentPath[pathIndex].WorldPosition.y,
            transform.position.z);

        Vector3 toTarget = targetPos - transform.position;
        float distance = toTarget.magnitude;
        if (distance < stopDistance)
        {
            rb.velocity = Vector2.zero;
            pathIndex++;
            return;
        }

        float speedFactor = distance < slowDownDistance ? distance / slowDownDistance : 1f;
        Vector2 desiredVelocity = toTarget.normalized * moveSpeed * speedFactor;
        rb.velocity = desiredVelocity;

        if (distance < stopDistance)
        {
            pathIndex++;
        }
    }
}
