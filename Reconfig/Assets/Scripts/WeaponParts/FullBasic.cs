using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicBase))]
[RequireComponent(typeof(BasicBarrel))]
[RequireComponent(typeof(BasicMagazine))]
[RequireComponent(typeof(BasicStock))]
[RequireComponent(typeof(BasicGrip))]
public class FullBasic : MonoBehaviour
{
    //does nothing just add to gameobject to add all basic parts
    //do this for other weopon types in the future to make dev easier
    //should remove this component after applying it but if you forget it will destroy itself
    void Start()
    {
        // After ensuring required basic parts are present, remove this helper component.
        Destroy(this);
    }
}
