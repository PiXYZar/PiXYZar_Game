using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour
{
    public float freezeTime = 5.0f;
    public GameObject platform;

    private float _timer;
    private bool _isFrozen;
    private MeshRenderer _renderer;


    public bool IsFrozen { get { return _isFrozen; } set { _isFrozen = value; } }

    void Start()
    {
        _timer = 0.0f;
        _isFrozen = false;
        _renderer = platform.GetComponent<MeshRenderer>();
        _renderer.enabled = false;
    }

    void FixedUpdate()
    {
        if (_isFrozen)
        {
            _timer += Time.deltaTime;

            if (_timer > freezeTime)
                _isFrozen = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
        {

        }

        _renderer.enabled = true;
    }
}
