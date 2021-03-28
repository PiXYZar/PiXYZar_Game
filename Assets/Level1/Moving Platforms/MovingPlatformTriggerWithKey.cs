using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformTriggerWithKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("player in trig area");
            if (other.gameObject.GetComponent<ThirdPersonController>().HasKey)
            {
                //Debug.Log("has key");
                //other.transform.parent = transform.parent;
                transform.parent.GetComponent<MovingPlatform>().goingtoend = true;
            }
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
            transform.parent.GetComponent<MovingPlatform>().isActive = false;
        }
    }
    */
}

