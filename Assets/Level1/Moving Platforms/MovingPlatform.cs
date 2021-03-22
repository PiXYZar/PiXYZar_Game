﻿using System.Collections;
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
    Quaternion lastDisplacer = Quaternion.Euler(0, 0, 0);
    
    public bool isActive = false;
    public bool isInContactWithPlayer = false;

    void Start() {
        origR = transform.rotation;
        endr = origR * Quaternion.Euler(endrV);

    }

    // Update is called once per frame
    void Update()
    {   
        if (isActive)
        {
            displacep = Vector3.Lerp(zeros, endp, phase);
            displacer = Quaternion.Lerp(origR, endr, phase);
            phase += speed * phaseDir * Time.deltaTime;
            amountp = displacep - lastDisplacep;
            transform.position += amountp;
            transform.rotation = displacer;
            
            if (isInContactWithPlayer)
            {
                //controller.Move(amount);
                //Will instead child the player to itself
            }
            lastDisplacep = displacep;
            lastDisplacer = displacer;
            if (phase >= 1 || phase <= 0)
            {
                phaseDir *= -1;
            }
        }
        
        isInContactWithPlayer = false;
    }

    public void playerHit()
    {
        isInContactWithPlayer = true;
    }
}
