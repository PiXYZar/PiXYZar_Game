using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //Vector3 startpos;
    //Vector3 endpos;
    public float speed = 0.5f;
    float phase = 0f;
    float phaseDir = 1f;
    Vector3 zeros = new Vector3(0, 0, 0);
    public Vector3 endp = new Vector3(0, 0, 18f);
    public Vector3 endrV = new Vector3(0, 90f, 0);
    Quaternion origR;
    Quaternion endr;
    Vector3 amountp;
    Quaternion amountr;
    Vector3 displacep;
    Quaternion displacer;
    Vector3 lastDisplacep = new Vector3(0, 0, 0);
    
    public bool isActive = false;
    public bool falling = false;
    public bool resetting = false;
    float m = 10;

    Rigidbody rb;

    void Start() {
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
        m += 30 * Time.deltaTime;
        transform.position -= transform.up * m * Time.deltaTime;
        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }
}
