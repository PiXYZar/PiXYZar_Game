using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour
{
    private Renderer _renderer;

    void Awake()
    {
        _renderer = transform.Find("Model").GetComponent<Renderer>();
        _renderer.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Bullet")
        {
            _renderer.enabled = true;
        }
    }
}
