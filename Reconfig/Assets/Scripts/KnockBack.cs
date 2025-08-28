using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockBack : MonoBehaviour
{
    public float knockbackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5f;
    public float inputFOrce = 7.5f;

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

        Vector2 _hitForce;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;

        _hitForce = hitDirection * hitDirectionForce;
        _constantForce = constantForceDirection * constForce;

        float _elapsedTime = 0f;
        while (_elapsedTime < knockbackTime)
        {
            // Iterate the timer
            _elapsedTime += Time.fixedDeltaTime;

            // Comebine _hitForce and _constantForce
            _knockbackForce = _hitForce + _constantForce;

            //comebing KnockbackForce with Input Force
            if (inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce;
            }

            // apply knockback
            rb.velocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockedBack = false;
    }

    public void callKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        knockBackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
    

}
