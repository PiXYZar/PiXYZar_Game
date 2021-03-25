using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float UpdateEnergyCapped(float current, float max, float rate)
    {
        if (current != max) 
        {
            current += rate * Time.deltaTime;
            if (current > max)
            {
                current = max;
            }
        }
        return current;
    }
}
