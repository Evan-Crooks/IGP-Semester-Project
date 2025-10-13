using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class testPrintInput : MonoBehaviour
{
    public TMP_InputField input;
    public string Action;

    public void PrintInput()
    {
        Debug.Log("Input field text: " + input.text);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get 
        //Make it so character can't type
        //Show default output
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
