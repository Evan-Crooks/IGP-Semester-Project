using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private float horizontal;
    private float speed = 8f;
    public float jumpingPower = 8f;//
    public bool movedSinceKnockback = false;

    private KnockBack knockBack;


    // Start is called before the first frame update
    void Start()
    {
        knockBack = GetComponent<KnockBack>();
    }

    void LateUpdate()
    {
        if (!knockBack.IsBeingKnockedBack)
        {
            if (movedSinceKnockback == true)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
        else
        {
            if (movedSinceKnockback == true)
            {
                movedSinceKnockback = false;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        if (!movedSinceKnockback)
        {
            movedSinceKnockback = true;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {        
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpingPower);
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f); // 0.2f should match your IsGrounded radius
        }
    }


}
