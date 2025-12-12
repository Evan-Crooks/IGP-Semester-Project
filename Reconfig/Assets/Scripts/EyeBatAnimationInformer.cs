using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBatAnimationInformer : MonoBehaviour
{
    [SerializeField] Animator ani;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    void Update()
    {
        if (rb == null || ani == null)
        {
            return;
        }

        ani.SetFloat("Velocity x", rb.velocity.x);
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = rb.velocity.x < 0f;
        }
        // TODO: update health
    }
}
