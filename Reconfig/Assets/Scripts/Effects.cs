using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    
/// <summary>
/// Applies a burning effect to the target GameObject, dealing damage over time.
/// </summary>
/// <param name="Target">The GameObject that will receive the burning damage</param>
/// <param name="damagePerTick">The amount of damage dealt each tick</param>
/// <param name="tickRate">The time interval in seconds between each damage tick</param>
/// <returns>An IEnumerator for coroutine execution</returns>
    IEnumerator Burn(GameObject Target, int damagePerTick, float tickRate)
    {
        yield break;
    }
}
