using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "player")
        {
            other.transform.parent = transform.parent;
            transform.parent.GetComponent<MovingPlatform>().isActive = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "player")
        {
            other.transform.parent = null;
            transform.parent.GetComponent<MovingPlatform>().isActive = false;
        }
    }
}
