using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLavaOnPortal : MonoBehaviour
{
    Transform planes;
    GameObject lava;
    // Start is called before the first frame update
    void Start()
    {
        planes = transform.Find("Planes");
        lava = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        planes.gameObject.SetActive(false);
        if (lava.transform.position.y > planes.position.y)
        {
            planes.gameObject.SetActive(true);
        }
    }
}
