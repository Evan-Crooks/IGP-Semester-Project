using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockBack : MonoBehaviour
{
    public float knockbackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5f;
    public float inputForce = 7.5f;

    private Rigidbody2D rb;

    private Coroutine knockBackCoroutine;

    public bool IsBeingKnockedBack { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        IsBeingKnockedBack = true;

        Vector2 hitForce = hitDirection * hitDirectionForce;
        Vector2 constantForce = constantForceDirection * constForce;
        Vector2 inputForceVec = inputDirection != 0 ? new Vector2(inputDirection * inputForce, 0f) : Vector2.zero;

        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            elapsedTime += Time.fixedDeltaTime;

            Vector2 knockbackForce = hitForce + constantForce + inputForceVec;
            rb.velocity = knockbackForce;

            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockedBack = false;
    }

    public void callKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        knockBackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
    

}
