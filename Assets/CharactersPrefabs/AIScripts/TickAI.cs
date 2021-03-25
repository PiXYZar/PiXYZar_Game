using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TickAI : MonoBehaviour
{
    // The position that the tick will move towards
    public Transform goal;

    //Vector3 start;
    Vector3 PlayerLocation;

    //float distanceToGoal;

    //This is a counter variable that determines when the tick should update its path planning. 1 = 1 second. 
    public float updatePath;

    Quaternion startRotation;
    Vector3 startPosition;

    public float movementSpeed = 2;
    public float attachDuration = 8;
    public float attackDelay = 4;
    public float massAddedToPlayer = 1;

    float attackDistance = 10.0f;

    //Booleans for determining animation and current action
    bool running = false;
    bool attacking = false;
    public bool attached = false;
    //bool turn = false;

    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        updatePath = 0;
        startRotation = transform.localRotation;
        startPosition = transform.localPosition;
        //start = transform.position;
        //distanceToGoal = 100;

        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Only update the path planning of 
        if (updatePath <= 0 && !attached)
        { 
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
        else if (updatePath <= 0 && attached)
        {
            Transform player = gameObject.transform.parent.parent;
            player.gameObject.GetComponent<Rigidbody>().mass = player.GetComponent<Rigidbody>().mass -= 1;
            gameObject.transform.parent.parent = null;
            attached = false;
            agent.enabled = true;
            agent.isStopped = false;
            transform.localRotation = startRotation;
            transform.localPosition = startPosition;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.transform.parent.parent = null;
            
        }
        else
        {
            // -1 every second
            updatePath = updatePath - 1 * Time.deltaTime;
        }
        animator.SetFloat("Speed", agent.speed);
        animator.SetBool("Attacking", attacking);
        animator.SetBool("Attached", attached);
    }

    public void collidedWithPlayer(GameObject other)
    {
        other.GetComponentInParent<Rigidbody>().mass = other.GetComponentInParent<Rigidbody>().mass += massAddedToPlayer;
        Debug.Log("attached");
        attached = true;
        running = false;
        attacking = false;
        agent.speed = 0;
        agent.isStopped = true;
        agent.enabled = false;
        updatePath = attachDuration;
        transform.parent.parent = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if player within trigger radius
        if (other.gameObject.tag == "Player")
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
                        running = false;
                        if (!attacking)
                        {
                            attacking = true;
                            agent.speed = 0;
                            agent.destination = PlayerLocation;
                            updatePath = 0.15f;
                        }
                        else
                        {
                            agent.speed = movementSpeed * 10;
                            attacking = false;
                            updatePath = attackDelay;
                            //gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
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
        // Check if player has left trigger area
        if (other.gameObject.tag == "Player")
        {
            running = false;
            attacking = false;
            attached = false;
        }
    }

}


