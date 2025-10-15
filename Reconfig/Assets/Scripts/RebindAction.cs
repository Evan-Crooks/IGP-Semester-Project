using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
//Code was created with the assistance of chatgpt. 
//Authors: 
//      - Connor Blaha(bad programmer)
//      - Chatgpt(also a bad programmer but knows stuff) <3
//This is why our ai overlords are going to take over someday
public class RebindAction : MonoBehaviour
{
    //Strings to store the physicsal
    // keys associated with a specific action or sub action of a composite action
    private string MoveLeft = "";
    private string MoveRight = "";
    private string Jump = "";
    private string Fire = "";

    //List of input fields that correspond to their actions
    public TMP_InputField inputMoveLeft;
    public TMP_InputField inputMoveRight;
    public TMP_InputField inputJump;
    public TMP_InputField inputFire;


    void Start()
    {
        //Grab Keys for movement to output to the thing
        var MoveActions = InputSystem.actions.FindAction("Move");
        foreach (var binding in MoveActions.bindings)
        {
            Debug.Log("Binding name: " + binding.name);
            if (binding.name == "left" || binding.name == "right")
            {
                //Get the key associated with left
                var keyPath = binding.effectivePath;
                //Split it so we can extract just the key from the keyboard
                var key = keyPath.Split('/')[1];
                //Assign it to move left or move right
                (binding.name == "left") ? MoveLeft = key : MoveRight = key;
                MoveLeft = key;
                Debug.Log("Move Left is: " + MoveLeft);
            }
        }
        //Grab key for jump
        // foreach (var action in MoveActions)
        // {
        //     Debug.Log(action);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        // {
        //     if (Input.GetKey(vKey))
        //     {
        //         Debug.Log(vKey + " was pressed");
        //     }
        // }
    }
}
