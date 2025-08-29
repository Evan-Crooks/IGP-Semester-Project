using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    private Vector2 moveInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        print(moveInput);
    }
    void FixedUpdate()
    {
        Vector3 movement = 5f * Time.fixedDeltaTime * new Vector2(moveInput.x, moveInput.y);
        transform.Translate(movement);
    }
}
