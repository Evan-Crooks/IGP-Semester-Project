using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float engageDistance = 10f;
    [SerializeField] private LayerMask lineOfSightMask = ~0; // colliders that block sight
    [SerializeField] EnemyMovementAI emAI;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
        if (emAI == null) emAI = gameObject.GetComponent<EnemyMovementAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= engageDistance && HasLineOfSight())
        {
            emAI.enabled = false;
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else emAI.enabled = true;
    }

    private bool HasLineOfSight()
    {
        Vector2 origin = transform.position;
        Vector2 dest = target.position;
        Vector2 dir = dest - origin;
        float dist = dir.magnitude;
        if (dist <= 0f)
        {
            return true;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, dir.normalized, dist, lineOfSightMask);
        return hit.collider == null || hit.transform == target;
    }
}
