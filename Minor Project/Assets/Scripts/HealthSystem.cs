//Author: Jasper van Bemmelen
//Version: 20/11/2016 22:08:00

using UnityEngine;
using System.Collections;

public abstract class HealthSystem : MonoBehaviour
{

    public float MaxHealth;     // Starting amount of Health
    public float CurHealth;     // Current amount of Health
	public float KnockbackForce;		// knockback force
	public float KnockbackFade;		//simulated character drag

    protected bool isDead;    // Variable to track death-ness 
    private bool damaged;  // Variable to track damaged-ness
	protected float CurHeight; //Variable to track height of an Object
	private bool knocked;	//checks whether the character must be knocked back
	private Vector3 direction; //direction of the object hitting the character
	private float impact;		//copy of KnockbackForce that fades over a set time using KnockbackFade
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
		if (knocked) {							//when the player must be knocked back
			gameObject.transform.Translate (direction * impact * Time.deltaTime, Space.World); //push back character with impact amount
			impact -= KnockbackFade;			//simulate drag by subtracting Knockbackfade from impact, every update

			if (impact <= 0) {					//when the impact is 0or lower (thus no more knockback)
				knocked = false;				//stop the function from executing
			}
		}
	}


    // Method to give the object damage
	public void TakeDamage( object[] temp ){	//takes an object array with on [0] the damage done(float) and on [1] the directopm vetor for knockback (Vector3)
        
		damaged = true;
		CurHealth -= (float) temp[0];
		
	if(gameObject.CompareTag("Player")){		//only run the knockback when a player is damaged
			direction = (Vector3)temp [1];		
			impact = KnockbackForce;			//copy the set KnockbackForce to a value used for calculation
			knocked = true;						//starts the knockback sequence while true
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
