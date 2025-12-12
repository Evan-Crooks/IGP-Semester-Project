using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossArrow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform source;
    void Start()
    {
        // Default to this transform if no explicit source is provided.
        // Prefer parent as source if present (e.g., arrow is a child of the shooter).
        if (transform.parent != null)
        {
            source = transform.parent;
        }
        source ??= transform;

        if (target == null)
        {
            GameObject boss = GameObject.Find("Boss");
            if (boss != null) target = boss.transform;
        }
        if (target == null)
        {
            GameObject bossSpawner = GameObject.Find("Boss Spawner");
            if (bossSpawner != null) target = bossSpawner.transform;
        }
    }
    void Update()
    {
        if (target == null || source == null)
        {
            return;
        }

        Vector2 direction = target.position - source.position ;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Flip when the source (or its parent) is mirrored on X.
        transform.localScale = transform.parent.localScale;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

}
