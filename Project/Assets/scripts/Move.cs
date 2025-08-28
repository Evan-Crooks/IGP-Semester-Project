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
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * 5f * Time.fixedDeltaTime;
        transform.Translate(movement);
    }
}
