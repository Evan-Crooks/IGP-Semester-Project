using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class EnemyWeapon : MonoBehaviour
{
    WeaponController wc;
    GameObject target;
    public float fireInterval = 1f;
    float fireTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        wc = gameObject.GetComponent<WeaponController>();
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval && CanSeeTarget())
        {
            Fire();
            fireTimer = 0f;
        }
    }
    // check if can see target
    bool CanSeeTarget()
    {
        if (target == null) return false;

        Vector2 origin = transform.position;
        Vector2 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.transform.position);

        int mask = ~ (1 << gameObject.layer); // ignore this GameObject's layer
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, mask);
        bool res = hit.collider != null && hit.collider.gameObject == target; 
        return res;
    }
    void Fire()
    {
        wc.Fire((target.transform.position - transform.position).normalized);
    }
}
