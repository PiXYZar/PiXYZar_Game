using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BlockShooterAI : MonoBehaviour
{

    public Transform goal;
    Vector3 start;
    Vector3 PlayerLocation;

    //float distanceToGoal;

    float updatePath;

    //int updateTime = 5;
    float movementSpeed = 1;

    //bool running = false;
    bool shooting = false;
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
        // Only update the
        //agent = GetComponent<NavMeshAgent>();
        if (updatePath <= 0)
        {

            if (shooting)
            {
                //anim.Play("run");
                shooting = false;
                agent.isStopped = false;
                agent.speed = 0.01f;
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
            updatePath = updatePath - 1 * Time.deltaTime;
        }
        animator.SetFloat("Speed", agent.speed);
        animator.SetBool("Shooting", shooting);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player 
        if (other.gameObject.layer == 8 || other.gameObject.layer == 11)
        {
            if (updatePath <= 0 && shooting == false)
            {
                shooting = true;
                PlayerLocation = other.gameObject.transform.position;
                agent.speed = 0.1f;
                agent.isStopped = false;
                updatePath = 1.4f;
            }
        }
    }
}