using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotatingPlatform : MonoBehaviour
{
    public GameObject tower;
    public float speed = 5.0f;
    public float rotationSpeed = 2.5f;

    //public float updateFrequencySeconds = 20f;

    private Rigidbody _rb;
    private Transform _modelTransform;

    //private float _timer = 0.0f;
    //private Vector2 _randomDir;

    // Start is called before the first frame update
    void Start()
    {
        // check if asset has rigid body, if yes, store it 
        if (GetComponent<Rigidbody>())
            _rb = GetComponent<Rigidbody>();
        else
            Debug.LogError("Player asset requires a rigid body component.");
        //_randomDir = Vector2.zero;

        _modelTransform = gameObject.transform.GetChild(0);

        Translate();
        Rotate(100);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*_timer += Time.deltaTime;
        if (_timer > updateFrequencySeconds)*/

        Translate();
        Rotate(rotationSpeed);
    }

    /*
    Vector2 RandomInput()
    {
        System.Random random = new System.Random();

        int first = random.Next(0, 2);
        int second = random.Next(0, 2);

        int xDir = first;
        if (first == 0)
            xDir = -1;

        int zDir = second;
        if (second == 0)
            zDir = -1;

        return new Vector2(xDir, zDir);
    }*/

    void Translate()
    {
        // local position of platform wrt tower centre 
        Vector3 platformLocal = tower.transform.InverseTransformPoint(transform.position);

        // calculate x and z values for platform movement 
        float hypotenuse = new Vector2(platformLocal.x, platformLocal.z).magnitude;
        float xDir = -platformLocal.z / hypotenuse;
        float zDir = platformLocal.x / hypotenuse;

        // movement 
        //float xVel = 0.0f;
        float zVel = 1.0f;       

        // create sideways and forward movement and combine them 
        //Quaternion targetRotation = Quaternion.Euler(0.0f, Mathf.Abs(xVel) * 90.0f, 0.0f);
        Vector3 forward = new Vector3(xDir * zVel, 0.0f, zDir * zVel);
        //Vector3 sideways = targetRotation * new Vector3(xDir * xVel, 0.0f, zDir * xVel);
        //_playerVel = (forward + sideways).normalized * speed;
        _rb.velocity = forward.normalized * speed;
    }

    void Rotate(float rotSpeed)
    {
        Quaternion targetRot = Quaternion.LookRotation(_rb.velocity, Vector3.up);
        targetRot = Quaternion.Euler(0.0f, 90.0f, 0.0f) * targetRot;
        _modelTransform.rotation = Quaternion.Lerp(_modelTransform.rotation, targetRot, rotSpeed * Time.deltaTime);
    }
    
}
