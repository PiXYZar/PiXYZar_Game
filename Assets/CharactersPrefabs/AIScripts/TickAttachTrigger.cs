using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickAttachTrigger : MonoBehaviour
{
    Transform dad;
    float dettachTime;

    private void Start()
    {
        dettachTime = 0;
    }

    private void Update()
    {
        dettachTime -= 1 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && dettachTime <= 0)
        {
            //dad = transform.parent.parent;
            //transform.parent.parent = other.transform;
            //transform.parent.GetComponent<TickAI>().attached = true;
            //transform.parent.GetComponent<TickAI>().updatePath = 6;
            transform.parent.GetComponent<TickAI>().collidedWithPlayer(other.gameObject);
            dettachTime = 12;
        }
    }
}
