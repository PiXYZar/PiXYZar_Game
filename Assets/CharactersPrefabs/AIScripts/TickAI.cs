using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TickAI : MonoBehaviour
{

    public Transform goal;
    Vector3 start;
    Vector3 PlayerLocation;

    //float distanceToGoal;

    int updatePath;

    //int updateTime = 5;
    float movementSpeed = 1;

    bool running = false;
    //bool turn = false;

    NavMeshAgent agent;
    Animator animator;


    void Start()
    {
        updatePath = 0;
        start = transform.position;
        //distanceToGoal = 100;


        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Only update the path planning of the penguin every couple of frames to slow down the penguins movement
        agent = GetComponent<NavMeshAgent>();
        if (updatePath <= 0)
        {
            // For when the penguin is fleeing from the player snowball instead of normal path planning
            if (running)
            {
                //anim.Play("run");
                agent.speed = movementSpeed * 2f;
                agent.destination = PlayerLocation;
            }
            else
            {
                agent.speed = 0;
                //anim.Play("walk");
                // Checks the distance to the goal and turns the penguin to walk back towards its start position once it gets close enough
                //if (distanceToGoal < 10.0f)
                //{
                //    turn = !turn;
                //}
            }

        }
        else
        {
            updatePath = updatePath - 1;
        }
        animator.SetFloat("Speed", agent.speed);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player snowball or falling rock, then run away
        if (other.gameObject.layer == 8 || other.gameObject.layer == 11)
        {
            if (updatePath <= 0)
            {
                running = true;
                PlayerLocation = other.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If snowball or rock is gone, resume normal path planning
        if (other.gameObject.layer == 8 || other.gameObject.layer == 11)
        {
            running = false;
        }
    }

}


