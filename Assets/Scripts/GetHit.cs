using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    [Tooltip("Determines when the player is taking damage.")]
    public bool hurt = false;
    
    [Tooltip("How much health is removed from the player when Hit")]
    [SerializeField] private int hitDamage = 0;
    //The current health level
    //private int health;
    private bool slipping = false;
    private PlayerMovement playerMovementScript;
    private Rigidbody rb;
    private Transform enemy;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        //stompEnemiesScript = GetComponent<StompEnemies>();
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        // stops the player from running up the slopes and skipping platforms
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
            playerMovementScript.playerStats.canMove = false;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (hurt == false)
        {
            if (other.gameObject.tag == "Enemy" && playerMovementScript.playerStats.canHurt)
            {
                enemy = other.gameObject.transform;
                rb.AddForce(enemy.forward * 1000);
                rb.AddForce(transform.up * 500);
                TakeDamage();
            }
            if (other.gameObject.tag == "Trap")
            {
                rb.AddForce(transform.forward * -1000);
                rb.AddForce(transform.up * 500);
                TakeDamage();
            }
        }
        if (other.gameObject.layer == 9)
        {
            slipping = true;
        }
        if (other.gameObject.layer != 9)
        {
            if (slipping == true)
            {
                slipping = false;
                playerMovementScript.playerStats.canMove = true;
            }
        }
    }
    private void TakeDamage()
    {
        hurt = true;
        playerMovementScript.playerStats.canMove = false;
        playerMovementScript.soundManager.PlayHitSound();
        //add in the reduction of health when hit
        HealthManager.TakeHealthHit(hitDamage);
        //HealthManager.TakeHealthHit();
        StartCoroutine("Recover");
    }
    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.75f);
        hurt = false;
        playerMovementScript.playerStats.canMove = true;
    }
}