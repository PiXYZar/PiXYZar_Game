using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour
{
    public float freezeTime = 5.0f;

    private float _timer;
    private bool _isFrozen;

    public bool IsFrozen { get { return _isFrozen; } set { _isFrozen = value; } }

    void Start()
    {
        _timer = 0.0f;
        _isFrozen = false;
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
}
