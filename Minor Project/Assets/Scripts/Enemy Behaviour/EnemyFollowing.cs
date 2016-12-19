using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyFollowing : HealthSystem
{
    public float followDistance = 10f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 3.0f;
    public float Damage = 1;
	public float teleportDistance = 0.8f;		//teleportDistance with respect to the player
	public float teleportRate = 2f;				//Once every teleportRate seconds the enemy teleports (Exponential Distribution)
	public float exponentialBias = 0;			//Bias for the exponential distribution
	public bool canTeleport = false;			//Boolean that allows teleporting
	public float teleportOrientation = 1;		//Before or behind the player
	public int Enemylife = 4;                   
	public GameObject Eggs;              
	public bool eggCapability;                  //determines if enemy is capable of laying eggs
	public int maxEggs = 5;                     //Max amount of eggs a enemy can lay
	public int Birthtime = 10;                  // ammount of seconds before enemy starts laying eggs
	public int MinBirthRepeatTime = 1;          // Minumum Birth repeatrate  
	public int MaxBirthRepeatTime = 10;         //Maximum Birth repeatrate

    private GameObject Player;					
    private Vector3 startPos;
	//Teleport variables
	private bool teleportReady;					//Boolean to be used with coroutine to check if enemy can teleport

    // Use this for initialization
    void Start()
    {
		int start_Birth = UnityEngine.Random.Range (1, Birthtime);
		int Birth_repeat = UnityEngine.Random.Range (MinBirthRepeatTime, MaxBirthRepeatTime);
		startPos = gameObject.transform.position;
		teleportReady = true;					//Enemy can teleport from the beginning
		InvokeRepeating("Lay_Eggs", start_Birth, Birth_repeat);
    }

    // Follow a 'player' if within followDistance
	protected override void Update()			//Now overrides the Update of HealthSystem to check y pos
	{
		base.Update ();							//Call to Update of Parent

		Player = GameObject.FindGameObjectsWithTag ("Player") [0];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;
		while (teleportReady && canTeleport) {
			teleportReady = false;
			IEnumerator coroutine = Teleport ();
			StartCoroutine(coroutine);
		}

		if (Vector3.Distance (playerPos, objectPos) < followDistance) {
			while (teleportReady && canTeleport) {			//If ready to teleport and enabled, teleport
				teleportReady = false;
				IEnumerator coroutine = Teleport ();
				StartCoroutine(coroutine);
			}
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (playerPos - objectPos), rotationSpeed * Time.deltaTime);
		} else {
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (startPos - objectPos), rotationSpeed * Time.deltaTime);
		}
		gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
	}

	private void OnCollisionStay( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( collision.gameObject.name + " Got Damaged");
            collision.gameObject.SendMessage( "TakeDamage", Damage );
        }
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            Enemylife -= 1;
            Destroy( collision.gameObject );
            if (Enemylife <= 0)
            {
                Destroy( this.gameObject );
            }
        }
    }

	private void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag("PlayerBullet")) {
			Enemylife -= 1;
			Destroy (col.gameObject);
			if (Enemylife == 0) {
				Destroy (this.gameObject);
			}
		}
	}

    protected override void Death()
    {
        Destroy(gameObject);
    }

	//Teleport method
	private IEnumerator Teleport() {
		float exponential = Uni2Exp (UnityEngine.Random.value, teleportRate);	//Draw exponentially distributed number (wait time)
		yield return new WaitForSeconds( exponential + exponentialBias );							//Wait for this long
		Vector3 playerPosition = Player.gameObject.transform.position;  		//Player position
		Quaternion playerRotation = Player.gameObject.transform.rotation;		//Player rotation
		Vector3 teleportDirection = playerRotation * Vector3.forward * teleportOrientation;				//Before the player
		Vector3 teleportLocation = teleportDirection * (gameObject.transform.position - playerPosition).magnitude * teleportDistance;	//Teleport location
		gameObject.transform.position = teleportLocation + playerPosition;					//Apply teleport
		gameObject.transform.rotation = Quaternion.LookRotation( teleportLocation );							//Turn the enemy in the same direction as player
		teleportReady = true;													//Enemy is ready to teleport again
	}

	//Inverse transform method: F(x) = y => F^-1(y) = x (Could also try Gaussian distribution?)
	protected float Uni2Exp(float uniform, float lambda) {
		return -(lambda) * Mathf.Log (uniform);					//if uniform~U(0,1) then for the returned value y: y~EXP(lambda) (lambda * exp( -lambda * y )
	}

	// Lays a random number of eggs
	void Lay_Eggs(){
		if (eggCapability) {
			int number = UnityEngine.Random.Range (1, maxEggs);
			for (int i = 0; i < number; i++) {

				Instantiate (Eggs, transform.position, transform.rotation);
			}
		}
	}
}