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

    private LavaScript _lava;

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
        _checkpointList[0].gameObject.GetComponent<Animator>().SetBool("LIT", true);

        _lava = GameObject.FindWithTag("Lava").GetComponent<LavaScript>();
    }
    
    void Update()
    {
        // if reached new checkpoint
        //Debug.Log(Vector3.Distance(transform.position, _currentCP));
        if (Vector3.Distance(transform.position, _currentCP) < checkpointThreshold)
        {
            if (_index == _checkpointList.Count - 1)
            {
                _checkpointList[_index - 1].gameObject.GetComponent<Animator>().SetBool("LIT", true);
                Debug.Log("[SUCCESS] You've reached the end!");
                _checkpointList[_index].gameObject.GetComponent<Animator>().SetBool("LIT", true);
            } 
            else
            {
                _index += 1;
                _previousCP = _checkpointList[_index - 1].position;
                _checkpointList[_index - 1].gameObject.GetComponent<Animator>().SetBool("LIT", false);
                _currentCP = _checkpointList[_index].position;
                _checkpointList[_index].gameObject.GetComponent<Animator>().SetBool("LIT", true);
                Debug.Log("[SUCCESS] Reached checkpoint number " + (_index - 1) + "/" + (_checkpointList.Count - 1));
            }
        }

        // if dead
        if (_dead)
        {
            Debug.Log("[DEAD] Restarting at checkpoint number " + (_index - 1) + "/" + (_checkpointList.Count - 1)); 
            transform.position = _previousCP;

            float lavaPosition = transform.position.y - 10.0f;
            if (lavaPosition < 0)
                lavaPosition = 0.0f;
            _lava.ResetToHeight(lavaPosition);

            _dead = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Lava")
        {
            _dead = true;
        }
    }
}
