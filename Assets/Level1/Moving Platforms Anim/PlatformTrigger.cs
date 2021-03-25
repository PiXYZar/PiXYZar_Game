using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public GameObject thePlatfrom;
    public GameObject thePlayer;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player")) 
            thePlayer.transform.parent = thePlatfrom.transform;
        //Debug.Log("Im in");
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("player")) 
            thePlayer.transform.parent = null;
        //Debug.Log("Im out");
    }
}
