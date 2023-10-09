using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorPatrol : MonoBehaviour
{
    /*
    Quick component to look after patrolling behaviour
    */
    [Tooltip("The transform to which the enemy will pace back and forth to.")]
    public Transform[] patrolPoints;
    
    private int currentPatrolPoint = 0;
    
    
    public void Patrol(float walkSpeed)
    {
        //Patrol Logic
        Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
        transform.position = Vector3.MoveTowards(transform.position, moveToPoint, walkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint > patrolPoints.Length - 1)
            {
                currentPatrolPoint = 0;
            }
        }
    }
}
