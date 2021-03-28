using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BlockShooterAI : MonoBehaviour
{
    Vector3 start;
    Vector3 PlayerLocation;

    // The position that the tick will move towards
    public Transform goal;


    float distanceToGoal;

    float updatePath;

    //int updateTime = 5;
    public float movementSpeed = 5;

    //bool running = false;
    bool shooting = false;
    bool walking = true;
    bool turn = false;

    public bool canWalk = true;

    NavMeshAgent agent;
    Animator animator;
    EnemyGun weaponScript;


    void Start()
    {
        updatePath = 0;
        start = transform.position;
        distanceToGoal = 100;

        weaponScript = GetComponentInChildren<EnemyGun>();
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = 0.1f;
    }

    private void Update()
    {
        // Only update the
        //agent = GetComponent<NavMeshAgent>();
        if (!shooting)
        {
            weaponScript.setActive(shooting);
            animator.SetBool("Shooting", shooting);
        }

        if (updatePath <= 0)
        {
            if (shooting)
            {
                //anim.Play("run");
                weaponScript.setActive(shooting);
                walking = false;
                shooting = false;
                agent.speed = 0f;
                agent.isStopped = true;
                this.transform.parent.LookAt(PlayerLocation);

                updatePath = 4;
            }
            else if (walking && canWalk)
            {
                if (distanceToGoal < 3.0f)
                {
                    turn = !turn;
                }
                agent.isStopped = false;
                agent.speed = movementSpeed;
                if (!turn)
                    agent.destination = goal.position;
                else
                    agent.destination = start;
                updatePath = 8;
            }
        }
        else
        {
            
            updatePath = updatePath - 1 * Time.deltaTime;
        }

        if (distanceToGoal <= 3.0f || !walking)
        {
            agent.speed = 0f;
        }

        //Checks the distance to the goal and turns the
        if (!turn)
            distanceToGoal = Mathf.Abs(transform.position.x - goal.position.x) + Mathf.Abs(transform.position.z - goal.position.z);
        else
            distanceToGoal = Mathf.Abs(transform.position.x - start.x) + Mathf.Abs(transform.position.z - start.z);

        animator.SetFloat("Speed", agent.speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            updatePath = 0.5f;
            walking = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player 
        if (other.gameObject.tag == "Player")
        {
            PlayerLocation = other.gameObject.transform.position;
            agent.destination = PlayerLocation;

            if (updatePath <= 0.5f && shooting == false)
            {
                shooting = true;
                agent.speed = 0f;
                agent.isStopped = false;
                animator.SetBool("Shooting", shooting);
                updatePath = 0.5f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            walking = true;
        }
    }
}