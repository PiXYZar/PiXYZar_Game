using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickAttachTrigger : MonoBehaviour
{
    Transform dad;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            //dad = transform.parent.parent;
            //transform.parent.parent = other.transform;
            //transform.parent.GetComponent<TickAI>().attached = true;
            //transform.parent.GetComponent<TickAI>().updatePath = 6;
            transform.parent.GetComponent<TickAI>().collidedWithPlayer(transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent.GetComponent<TickAI>().stopCollisionWithPlayer(transform);
            //transform.parent.parent = dad;
            //transform.parent.GetComponent<TickAI>().attached = false;
        }
    }
}
