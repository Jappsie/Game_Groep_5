//Author: Jasper van Bemmelen
//Version: 20/11/2016 22:08:00

using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{

    public float MaxHealth;     // Starting amount of Health
    private float CurHealth;     // Current amount of Health
    PlayerMovement playerMovement;  // Reference to playermovement script

    private bool isDead;    // Variable to track death-ness 
    private bool damaged;   // Variable to track damaged-ness

    // Get reference to playerMovement and revive object
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        CurHealth = MaxHealth;
        isDead = false;
    }

    // Method to give the object damage
    public void TakeDamage( float damageAmount )
    {
        damaged = true;
        CurHealth -= damageAmount;

        // Check for negative health and if not dead yet, invoke death
        if ( CurHealth <= 0 && !isDead )
        {
            Death();
        }
    }

    // Method to provide effects of death
    void Death()
    {
        isDead = true;
        playerMovement.isDead = true;
    }
}
