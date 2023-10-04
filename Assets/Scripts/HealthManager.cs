using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    //The current health level
    private static float health;
    private static float maxHealth;
    private static int coinCount = 0;
    //how many coins make up a single health point
    private static int coinPoint = 10;

    public static void TakeHealthHit(float hitDamage)
    {
        health -= hitDamage;
        if(health <= 0){
            Debug.Log("WE HAVE NO HEALTH REMAINING");
            health = 0;
        }else{
            PrintHealth();
        }
    }

    private static void PrintHealth()
    {
        Debug.Log($"Health: {health}");
    }
    public static void ResetHealth(float startingHealth)
    {
        health = startingHealth;
        maxHealth = startingHealth;
    }

    public static void AddCoinToHealth()
    {
        coinCount += 1;
        Debug.Log($"Just {coinPoint-coinCount} more coins and you get a health bonus point");
        if(coinCount >= coinPoint){
            coinCount = 0;
            health = (health < maxHealth) ? health + 1 : health;
            PrintHealth();
        }
    }
}
