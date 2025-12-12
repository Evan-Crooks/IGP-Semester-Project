using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpAndDown : MonoBehaviour
{
    [SerializeField] private Vector3 floatRangeAndRate;

    void Update()
    {
        // floatRangeAndRate: x = minY, y = maxY, z = speed.
        float minY = Mathf.Min(floatRangeAndRate.x, floatRangeAndRate.y);
        float maxY = Mathf.Max(floatRangeAndRate.x, floatRangeAndRate.y);
        float speed = floatRangeAndRate.z;

        float range = Mathf.Max(maxY - minY, 0f);
        float yPos = Mathf.PingPong(Time.time * speed, range) + minY;

        Vector3 pos = transform.localPosition;
        pos.y = yPos;
        transform.localPosition = pos;
    }
}
