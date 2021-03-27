using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("End position/rotation")]
    public Vector3 endp = new Vector3(0, 0, 0);
    public Vector3 endrV = new Vector3(0, 0, 0);
    public float speed = 0f;
    public float fall_speed = 0.05f;

    // Working vars
    float phase = 0f;
    float phaseDir = 1f;
    Vector3 zeros = new Vector3(0, 0, 0);
    Vector3 origP;
    Quaternion origR;
    Quaternion endr;
    Vector3 amountp;
    Vector3 displacep;
    Quaternion displacer;
    Vector3 lastDisplacep = new Vector3(0, 0, 0);
    Rigidbody rb;
    float orig_fall_speed;

    [Header("State toggles")]
    public bool isActive = false;
    public bool falling = false;
    public bool goingtobeginning = false;
    public bool goingtoend = false;

    void Start() {
        origP = transform.position;
        origR = transform.rotation;
        endr = origR * Quaternion.Euler(endrV);
        rb = gameObject.GetComponent<Rigidbody>();
        orig_fall_speed = fall_speed;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!falling && (isActive || goingtobeginning || goingtoend))
        {
            MoveAround();
        }
        if (falling)
        {
            Fall();
        }
    }

    void MoveAround()
    {
        if (goingtobeginning) 
        {
            phaseDir = -1;
            if (phase <= 0)
            {
                goingtobeginning = false;
            }
        }
        else if (goingtoend) 
        {
            phaseDir = 1;
            if (phase >= 1)
            {
                goingtoend = false;
            }
        }

        displacep = Vector3.Lerp(zeros, endp, phase);
        displacer = Quaternion.Lerp(origR, endr, phase);
        
        phase += speed * phaseDir * Time.deltaTime;

        amountp = displacep - lastDisplacep;
        transform.position += amountp;
        transform.rotation = displacer;
        
        lastDisplacep = displacep;
        
        if (phase >= 1)
        {
            phaseDir = -1;
        }
        else if (phase <= 0)
        {
            phaseDir = 1;
        }
    }

    void Fall()
    {
        fall_speed += fall_speed * Time.deltaTime;
        transform.position -= transform.up * fall_speed * Time.deltaTime;
        if (transform.position.y < -100)
        {
            falling = false;
        }
    }

    public void Reset()
    {
        transform.position = origP;
        transform.rotation = origR;
        fall_speed = orig_fall_speed;
        phase = 0;
        falling = false;
        isActive = false;
    }
}
