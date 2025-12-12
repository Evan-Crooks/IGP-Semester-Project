using UnityEngine;
using UnityEngine.InputSystem;

public class HideMouse : MonoBehaviour
{
    //reference to the custom cursor made with a canvas
    public Transform cursorVisual;
    //displacement from the mouse position
    public Vector2 displacement;
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        cursorVisual.position = mousePosition + displacement;

    }
}
