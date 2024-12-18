using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    GameObject target;
    Player playerScript;

    NavMeshPath path;
    Vector3 startPos;

    [SerializeField] Vector3 selectedTarget;

    int randomTarget;
    float distance; // Distance between Enemy and Player

    void Start()
    {       
        path = new NavMeshPath();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = 3f;

        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<Player>();

        startPos = transform.position;

        InvokeRepeating("SetRandomSpeed", 5f, 5f); // Vary the speed every X seconds
        InvokeRepeating("SetNewRandomTarget", 5f, 5f); // Vary the targets every X seconds

        randomTarget = Random.Range(1, 4);
    }

    void Update()
    {       
        if (target != null)
        {
            UpdateRandomTarget();

            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance > 5f) // If distance between Enemy and Player is greater than X
            {
                navMeshAgent.CalculatePath(target.transform.position, path);

                if (path.status == NavMeshPathStatus.PathComplete) // First checks if there is a path to the Player
                {
                    navMeshAgent.CalculatePath(selectedTarget, path);

                    if (path.status == NavMeshPathStatus.PathComplete) // Checks for a path to one of the selected targets
                    {
                        navMeshAgent.SetDestination(selectedTarget); // Move to randomly selected target
                    }  
                    else
                    {
                        SetNewRandomTarget(); // Set new random target if Path is unreachable
                    }
                }
                else
                {
                    navMeshAgent.SetDestination(startPos); // Go back to starting position - if Path is unreachable
                }
            }
            else // If enemy is close to Player
            {
                navMeshAgent.CalculatePath(target.transform.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    navMeshAgent.SetDestination(target.transform.position);
                }
                else
                {
                    navMeshAgent.SetDestination(startPos); // Go back to starting position - if Path is unreachable
                }
            }
        }
        else
        {
            navMeshAgent.SetDestination(startPos); // Go back to starting position - if Player is missing/dead
        }
        
        //Debug.Log("Distance: " + distance);
    }   

    void SetRandomSpeed()
    {
        int randomSpeed = Random.Range(2, 4);
        navMeshAgent.speed = randomSpeed;
    }

    void UpdateRandomTarget()
    {
        if (randomTarget == 1)
        {
            selectedTarget = target.transform.position; // The Player Object
        }
        else if (randomTarget == 2)
        {
            selectedTarget = playerScript.positiveVelocityDirPos; // The Player's velocity direction
        }
        else if (randomTarget == 3)
        {
            selectedTarget = playerScript.negativeVelocityDirPos; // The Player's velocity direction
        }
    }

    void SetNewRandomTarget()
    {
        randomTarget = Random.Range(1, 4);
    }

    void OnDrawGizmos() // Debugging
    {
        if (path != null)
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.cyan);
            }
        }        
    }
}
