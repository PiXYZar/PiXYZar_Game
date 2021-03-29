using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFallingTrigger : MonoBehaviour
{
    public Transform platform;
    
    private void OnTriggerEnter(Collider other) {
        platform.GetComponent<MovingPlatform>().Reset();
    }
}
