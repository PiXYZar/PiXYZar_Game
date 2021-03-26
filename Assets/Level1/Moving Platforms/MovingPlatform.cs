using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 0.5f;
    [Header("End position/rotation")]
    public Vector3 endp = new Vector3(0, 0, 18f);
    public Vector3 endrV = new Vector3(0, 90f, 0);

    float phase = 0f;
    float phaseDir = 1f;
    Vector3 zeros = new Vector3(0, 0, 0);
    Vector3 origP;
    Quaternion origR;
    Quaternion endr;
    Vector3 amountp;
    Quaternion amountr;
    Vector3 displacep;
    Quaternion displacer;
    Vector3 lastDisplacep = new Vector3(0, 0, 0);
    
    [Header("State toggles")]
    public bool isActive = false;
    public bool falling = false;
    public bool resetting = false;
    float fall_speed = 10;

    Rigidbody rb;

    void Start() {
        origP = transform.position;
        origR = transform.rotation;
        endr = origR * Quaternion.Euler(endrV);

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!falling && (isActive || resetting))
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
        if (resetting) 
        {
            phaseDir = -1;
            if (phase <= 0)
            {
                resetting = false;
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
        fall_speed += 30 * Time.deltaTime;
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
        phase = 0;
    }
}
