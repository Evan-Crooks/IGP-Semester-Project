using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class followCursor : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    Vector3 defaultScale = new Vector3(-1,1,1);
    void Update()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);
    
        if(transform.parent.localScale.x < 0)
        {
            transform.localScale = defaultScale * -1;
        }
        else transform.localScale = defaultScale;

        AimAtTarget(mouseWorld);
    }

    public void AimAtTarget(Vector3 targetWorld)
    {
        Vector2 toTarget = targetWorld - transform.position;
        if (toTarget.sqrMagnitude < Mathf.Epsilon)
            return;

        float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
