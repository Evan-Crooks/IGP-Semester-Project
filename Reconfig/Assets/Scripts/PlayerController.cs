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
    [SerializeField] float speed = 1f;
    public float jumpingPower = 8f;//
    public bool movedSinceKnockback = false;

    private KnockBack knockBack;
    Animator ani;
    PlayerHealth health;


    // Start is called before the first frame update
    void Start()
    {
        knockBack = GetComponent<KnockBack>();
        ani = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        ani.SetFloat("Velocity x", rb.velocity.x/10);
        ani.SetFloat("Velocity y", rb.velocity.y);
        ani.SetFloat("Health", health.health);
        if(rb.velocity.x < 0) transform.localScale = new Vector3(-1,1,1);
        if(rb.velocity.x > 0) transform.localScale = Vector3.one;
    }

    void LateUpdate()
    {
        if (!knockBack.IsBeingKnockedBack)
        {
            if (movedSinceKnockback == true)
            {
                rb.velocity += new Vector2(horizontal * speed, 0);
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
        Debug.Log("Jump input detected");
        Debug.Log("IsGrounded: " + IsGrounded());
        if (context.performed && IsGrounded())
        {
            rb.velocity += Vector2.up*jumpingPower;
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
