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

    float updateWalk;

    //int updateTime = 5;
    float movementSpeed = 5;

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
        updateWalk = 0;
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
                walking = false;
                agent.speed = movementSpeed;
                float x = Random.Range(-5, 5);
                float y = Random.Range(-5, 5);
                Vector3 walkLocation = new Vector3(x,y,0);
                agent.destination = transform.position + walkLocation;
                updateWalk = 10;
                
            }
            else if (updateWalk <= 5 && updateWalk >= 0)
            {
                agent.speed = 0.1f;
            }
            else if(updateWalk <= 0)
            {
                walking = true;
            }

        }
        else
        {
            updateWalk = updateWalk - 1 * Time.deltaTime;
            updatePath = updatePath - 1 * Time.deltaTime;
        }
        animator.SetFloat("Speed", agent.speed);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check within trigger radius for the player 
        if (other.gameObject.tag == "Player")
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