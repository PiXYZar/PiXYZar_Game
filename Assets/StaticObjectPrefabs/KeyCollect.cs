using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyCollect : MonoBehaviour
{

    public bool canYouPickupKey = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (canYouPickupKey)
            {
                other.gameObject.GetComponent<ThirdPersonController>().HasKey = true;
                Destroy(gameObject);
            }
            else
            {
                if (other.gameObject.GetComponent<ThirdPersonController>().HasKey)
                {
                    other.gameObject.GetComponent<ThirdPersonController>().HasKey = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}
