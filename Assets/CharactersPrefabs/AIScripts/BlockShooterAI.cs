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
    bool walking = false;
    //bool turn = false;

    NavMeshAgent agent;
    Animator animator;
    EnemyGun weaponScript;


    void Start()
    {
        updatePath = 0;
        start = transform.position;
        //distanceToGoal = 100;

        weaponScript = GetComponentInChildren<EnemyGun>();
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
                shooting = false;
                agent.isStopped = false;
                agent.speed = 0.1f;
                agent.destination = PlayerLocation;
                
                updatePath = 4;
            }
            else if (walking)
            {

            }
            else
            {
                agent.speed = 0.1f;
                //agent.isStopped = true;
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
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player 
        if (other.gameObject.layer == 8)
        {
            PlayerLocation = other.gameObject.transform.position;
            agent.destination = PlayerLocation;

            if (updatePath <= 0 && shooting == false)
            {
                shooting = true;
                agent.speed = 0.1f;
                agent.isStopped = false;
                animator.SetBool("Shooting", shooting);
                updatePath = 0.5f;
            }
        }
    }
}