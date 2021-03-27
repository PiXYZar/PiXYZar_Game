using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour
{
    public float freezeTime = 5.0f;
    public GameObject platform;

    private TickAI _ticker;
    private PusherAI _pusher;

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
            if (_pusher != null)
            {
                //_pusher.agent.speed = 0.0f;
                //_pusher.agent.isStopped = true;
            }

            if (_ticker != null)
            {
                //_ticker.agent.speed = 0.0f;
            }

            _timer += Time.deltaTime;

            if (_timer > freezeTime)
                _isFrozen = false;
        }
        else
        {
            _pusher = null;
            _ticker = null;
        }
    }

    void onTriggerEnter(Collision collider)
    {
        if (!_isFrozen)
        {
            if (collider.gameObject.name == "PusherEnemy")
            {
                _pusher = collider.gameObject.GetComponentInChildren<PusherAI>();
                _isFrozen = true;
            }
            else if (collider.gameObject.name == "TickEnemy")
            {
                _ticker = collider.gameObject.GetComponentInChildren<TickAI>();
                _isFrozen = true;
            }
        }

        _renderer.enabled = true;
    }
}
