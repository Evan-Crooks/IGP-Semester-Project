using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBatAnimationInformer : MonoBehaviour
{
    [SerializeField] Animator ani;
    [SerializeField] Rigidbody2D rb;
    void Update()
    {
        ani.SetFloat("Velocity x", rb.velocity.x);
        // TODO: update health
    }
}
