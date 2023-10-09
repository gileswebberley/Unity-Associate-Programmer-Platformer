﻿using System.Collections;
using UnityEngine;

public class RefactorEnemy : MonoBehaviour
{
    public Stats enemyStats;
    
    /// <summary>
    /// Contains tunable parameters to tweak the enemy's movement and behavior.
    /// </summary>
    [System.Serializable]
    public struct Stats
    {
        [Header("Enemy Settings")]
        [Tooltip("How fast the enemy walks (only when idle is true).")]
        public float walkSpeed;

        [Tooltip("How fast the enemy turns in circles as they're walking (only when idle is true).")]
        public float rotateSpeed;

        [Tooltip("How fast the enemy runs after the player (only when idle is false).")]
        public float chaseSpeed;

        [Tooltip("How close the enemy needs to be to explode")]
        public float explodeDist;

    }

    [Tooltip("The transform that will lock onto the player once the enemy has spotted them.")]
    public Transform sight;

    

    [Tooltip("Blue explosion particles")]
    public GameObject enemyExplosionParticles;


    private bool slipping = false;
    //Whether the enemy is idle or not. Once the player is within distance, idle will turn false and the enemy will chase the player.
    private bool idle = true;

    private GameObject  player;
    private RefactorPatrol patroller;
    //flag to stop repeated exploding
    private bool isExploding = false;

    private void Start()
    {
        //get the existing patrolling class
        patroller = GetComponent<RefactorPatrol>();
        
    }
    private void Update()
    {
        // changes the enemy's behavior: pacing in circles or chasing the player
        if (idle)
        {
            patroller.Patrol(enemyStats.walkSpeed);
        }
        else
        {
            CheckChaseOrFlee();
            CheckExplode();
        }

        CheckSlipping();
        
    }

    private void CheckSlipping()
    {
        // stops enemy from following player up the inaccessible slopes
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
        }
    }

    private void CheckChaseOrFlee()
    {
        if(HealthManager.GetHealthPercent() >= 100f)
        {
            Flee();
        }
        else{
            Chase();
        }
    }

    private void CheckExplode()
    {
        //Explode if we get within the enemyStats.explodeDist
        if (Vector3.Distance(transform.position, player.transform.position) < enemyStats.explodeDist && !isExploding)
        {
            isExploding = true;
            StartCoroutine("Explode");
            idle = true;
        }
    }

    private void Flee()
    {
        //Debug.Log("FLEE");
        //run away from the player
        sight.position = new Vector3(-player.transform.position.x, transform.position.y, -player.transform.position.z);
        transform.LookAt(sight);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * enemyStats.chaseSpeed*-1);
    }

    private void Chase()
    {
        //Debug.Log("CHASE");
        //Chase the player
        sight.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(sight);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * enemyStats.chaseSpeed);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            slipping = true;
        }
        else
        {
            slipping = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //start chasing if the player gets close enough
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            idle = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //stop chasing if the player gets far enough away
        if (other.gameObject.tag == "Player")
        {
            idle = true;
        }
    }

    private IEnumerator Explode()
    {
        GameObject particles = Instantiate(enemyExplosionParticles, transform.position, new Quaternion());
        yield return new WaitForSeconds(0.2f);
        Destroy(transform.parent.gameObject);
    }


}