using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawner : MonoBehaviour
{
    public GameObject checkpoints;
    public float checkpointThreshold = 2.0f;

    private List<Transform> _checkpointList;
    private Vector3 _previousCP;
    private Vector3 _currentCP;
    private bool _dead;
    private int _index;

    public bool Dead { get { return _dead; } }
    
    void Start()
    {
        // add checkpoints to list 
        _checkpointList = new List<Transform>();
        foreach (Transform child in checkpoints.transform)
        {
            _checkpointList.Add(child);
        }

        _dead = false;
        _index = 1;
        transform.position = _previousCP = _checkpointList[_index - 1].position;
        _currentCP = _checkpointList[_index].position;
    }
    
    void Update()
    {
        // if reached new checkpoint
        //Debug.Log(Vector3.Distance(transform.position, _currentCP));
        if (Vector3.Distance(transform.position, _currentCP) < checkpointThreshold)
        {
            if (_index == _checkpointList.Count)
            {
                Debug.Log("[SUCCESS] You've reached the end!");
            } 
            else
            {
                _index += 1;
                _previousCP = _checkpointList[_index - 1].position;
                _currentCP = _checkpointList[_index].position;
                Debug.Log("[SUCCESS] Reached checkpoint number " + (_index - 1) + "/" + _checkpointList.Count);
            }
        }

        // if dead
        if (_dead)
        {
            Debug.Log("[DEAD] Restarting at checkpoint number " + (_index - 1) + "/" + _checkpointList.Count); 
            transform.position = _previousCP;
            _dead = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Lava")
        {
            _dead = true;
        }
    }
}
