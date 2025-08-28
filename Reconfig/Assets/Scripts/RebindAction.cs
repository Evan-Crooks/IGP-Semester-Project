using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RebindAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Figred out how to retrieve actions from input system, need to continue later
        var MoveBindings = InputSystem.actions.FindAction("Move").bindings;
        for (int i = 0; i < MoveBindings.Count; i++)
        {
            Debug.Log(MoveBindings[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                Debug.Log(vKey + " was pressed");
            }
        }
    }
}
