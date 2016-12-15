//Author: Jasper van Bemmelen
//Version: 20/11/2016 22:08:00

using UnityEngine;
using System.Collections;

public abstract class HealthSystem : MonoBehaviour
{

    public float MaxHealth;     // Starting amount of Health
    public float CurHealth;     // Current amount of Health
    public float deathHeight;   // Consider the player dead if it falls below this threshold

    protected bool isDead;    // Variable to track death-ness 
    private bool damaged;  // Variable to track damaged-ness
	protected float CurHeight; //Variable to track height of an Object

    // Get reference to playerMovement and revive object
    void Awake()
    {
        Debug.Log( "Revived " + gameObject.name);
        CurHealth = MaxHealth;
        isDead = false;
		damaged = false;
		CurHeight = gameObject.transform.position.y;
    }

	protected virtual void Update() {			//virtual means it can be overriden by child class
		CurHeight = gameObject.transform.position.y;
		if (CurHeight < deathHeight) {				//Whenever the object is below -10.0f, its death function gets called
			Death ();
		}
	}

    // Method to give the object damage
    public void TakeDamage( float damageAmount )
    {
        damaged = true;
        CurHealth -= damageAmount;

        // Check for negative health and if not dead yet, invoke death
        if ( CurHealth <= 0 && !isDead )
        {
            isDead = true;
            Death();
        }
    }

    // Method to provide effects of death
    protected abstract void Death();
}
