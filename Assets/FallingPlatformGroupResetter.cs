using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformGroupResetter : MonoBehaviour
{
    public void resetPlatforms()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform c = transform.GetChild(i);
            c.GetComponent<MovingPlatform>().Reset();
        }
    }
}
