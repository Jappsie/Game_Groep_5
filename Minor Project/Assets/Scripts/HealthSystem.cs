//Author: Jasper van Bemmelen
//Version: 20/11/2016 22:08:00

using UnityEngine;
using System.Collections;

public abstract class HealthSystem : MonoBehaviour
{

    public float MaxHealth;     // Starting amount of Health
    public float CurHealth;     // Current amount of Health
	public float KnockbackForce;		// knockback factor
	public float KnockbackDistance;		// knockback distance

    protected bool isDead;    // Variable to track death-ness 
    private bool damaged;  // Variable to track damaged-ness
	protected float CurHeight; //Variable to track height of an Object
	private bool knocked;
	private Vector3 direction;
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
		if (CurHeight < -10.0f) {				//Whenever the object is below -10.0f, its death function gets called
			Death ();
		}
	}
	void FixedUpdate(){
		if (knocked) {
			gameObject.transform.Translate (-1f * direction * KnockbackForce * Time.deltaTime);
			if (gameObject.transform.Equals((-1f * direction * KnockbackDistance) + gameObject.transform.position)){
				knocked = false;
			}
		}
	}

    // Method to give the object damage
	public void TakeDamage( object[] temp )
    {
        damaged = true;
		CurHealth -= (float) temp[0];
		if(gameObject.CompareTag("Player")){
			direction = (Vector3)temp [1];
			knocked = true;
			//gameObject.transform.Translate (-1f * (Vector3)temp [1] * Knockback * Time.deltaTime);
		}

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
