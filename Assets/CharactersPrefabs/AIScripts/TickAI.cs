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

    float updatePath;

    //int updateTime = 5;
    float movementSpeed = 2;

    float attackDistance = 10.0f;

    bool running = false;
    bool attacking = false;
    bool attached = false;
    //bool turn = false;

    NavMeshAgent agent;
    Animator animator;


    void Start()
    {
        updatePath = 0;
        start = transform.position;
        //distanceToGoal = 100;


        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Only update the path planning of the penguin every couple of frames to slow down the penguins movement
        if (updatePath <= 0 && !agent.isStopped)
        {
            gameObject.GetComponentInParent<Rigidbody>().isKinematic = false;
            // For when the penguin is fleeing from the player snowball instead of normal path planning
            if (running)
            {
                //anim.Play("run");
             
                agent.speed = movementSpeed;
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
        animator.SetBool("Attacking", attacking);
        animator.SetBool("Attached", attached);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Check if its the player
        if (other.gameObject.layer == 8)
        {
            attached = true;
            running = false;
            attacking = false;
            gameObject.transform.parent.parent = other.gameObject.transform;
            agent.speed = 0;
            agent.isStopped = true;
            agent.enabled = false;
            gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        //Check if its the player
        if (other.gameObject.layer == 8)
        {
            //attached = false;
            //agent.enabled = true;
            //gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player snowball or falling rock, then run away
        if (other.gameObject.layer == 8 || other.gameObject.layer == 11)
        {
            if (updatePath <= 0)
            {
                if (!attached)
                {
                    agent.isStopped = false;
                    running = true;
                    PlayerLocation = other.gameObject.transform.position;

                    if (Mathf.Abs((PlayerLocation.x + PlayerLocation.z) - (transform.position.x + transform.position.z)) <= attackDistance)
                    {
                        if (!attacking)
                        {
                            attacking = true;
                            agent.speed = 0;
                            updatePath = 0.15f;
                        }
                        else
                        {
                            agent.speed = movementSpeed * 7;
                            updatePath = 4f;
                            gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
                        }
                    }
                    else
                    {
                        attacking = false;

                    }
                }
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


