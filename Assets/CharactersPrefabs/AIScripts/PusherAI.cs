using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PusherAI : MonoBehaviour
{

    public Transform goal;
    Vector3 start;
    Vector3 PlayerLocation;

    //float distanceToGoal;

    Quaternion startRotation;
    Vector3 startPosition;

    float updatePath;

    float movementSpeed = 1;

    bool charging = false;
    //bool turn = false;

    NavMeshAgent agent;
    Animator animator;


    void Start()
    {
        updatePath = 0;
        start = transform.position;
        //distanceToGoal = 100;

        startRotation = transform.localRotation;
        startPosition = transform.localPosition;


        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Only update the
        //agent = GetComponent<NavMeshAgent>();
        if (updatePath <= 0)
        {
            
            if (charging)
            {
                //anim.Play("run");
                charging = false;
                agent.isStopped = false;
                transform.localRotation = startRotation;
                transform.localPosition = startPosition;
                agent.speed = movementSpeed*11;
                agent.destination = PlayerLocation;
                updatePath = 4;
            }
            else
            {
                agent.speed = 0;
                agent.isStopped = true;
                //anim.Play("walk");
                // Checks the distance to the goal and turns the 
                //if (distanceToGoal < 10.0f)
                //{
                //    turn = !turn;
                //}
            }

        }
        else
        {
            updatePath = updatePath - 1*Time.deltaTime;
        }
        animator.SetFloat("Speed", agent.speed);
        animator.SetBool("Charging", charging);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player 
        if (other.gameObject.tag == "Player")
        {
            if (updatePath <= 0 && charging == false)
            {
                charging = true;
                PlayerLocation = other.gameObject.transform.position;
                agent.speed = 0.1f;
                agent.isStopped = false;
                updatePath = 1.4f;
            }
            if (charging)
            {
                PlayerLocation = other.gameObject.transform.position;
                agent.destination = PlayerLocation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If player is gone, resume normal path planning
        //if (other.gameObject.layer == 8 || other.gameObject.layer == 11)
        //{
        //    running = false;
        //}
    }

}


