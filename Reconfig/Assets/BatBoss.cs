using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BatBoss : MonoBehaviour
{
    Health health;
    [SerializeField] Behaviour[] Redcomponents;
    MeleeEnemy meleeEnemy;

    Animator ani;
    [SerializeField] RuntimeAnimatorController blackanimation;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        meleeEnemy = GetComponent<MeleeEnemy>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.health <= health.maxHealth / 2)
        {
            print($"{health.health}/{health.maxHealth}");
            foreach(Behaviour c in Redcomponents) c.enabled = false;
            meleeEnemy.enabled = true;
            ani.runtimeAnimatorController = blackanimation;
        }

    }
}
