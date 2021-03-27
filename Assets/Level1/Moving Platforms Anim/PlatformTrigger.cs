using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public GameObject thePlatfrom;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
            other.transform.parent = thePlatfrom.transform;
        //Debug.Log("Im in");
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
            other.transform.parent = null;
        //Debug.Log("Im out");
    }
}
