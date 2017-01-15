using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	public float triggertime = 2f;
	public float repeatrate = 1f;
	public float LineofSight = 10f;
	public float rotationSpeed = 3.0f;
	public GameObject bullet;
	public int TurretLife = 3;
	public bool adaptive = false;

	protected Vector3 startPos;
	protected Quaternion startRot;
	protected GameObject Player;

	private int deaths;

	// Use this for initialization
	protected virtual void Start () 
	{
		//Start position + rotation of the turret
		startPos = gameObject.transform.position;
		startRot = gameObject.transform.rotation;

		//Make repeatrate a logistic function of the amount of deaths
		deaths = SceneManagerScript.deathList.Count;
		//When deaths == 0, repeatrate is equal to itself
		if (adaptive) {
			repeatrate = 2f * repeatrate * (1 / (1 + Mathf.Exp (-0.3f * deaths)));
			Debug.Log ("RepeatRate: " + repeatrate + "!!!!!!!!!!!!!!!!!!!");
		}

		// Call BulletTrigger every so often
		InvokeRepeating("BulletTrigger",triggertime,repeatrate);

	}
	

	void Update () {
		// Aim at the player

		Player = GameObject.FindGameObjectsWithTag ("Player") [0];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;

        //Destroys Turret if Turrerlife equals zero
        if ( TurretLife == 0) {
			Destroy (this.gameObject);
		}

		if (Vector3.Distance (playerPos, objectPos) < LineofSight) {
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (playerPos - objectPos), rotationSpeed * Time.deltaTime);
		}
		// Return to start position if no player is found
		else {
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, startRot, rotationSpeed * Time.deltaTime);
		}

	}	

	//Checks if the turret is hit by the player bullet
	private void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag("PlayerBullet")) {
			TurretLife -= 1;
			Destroy (col.gameObject);
		}
	}
		
	


	virtual protected void BulletTrigger(){
		// Fire a bullet

		Player = GameObject.FindGameObjectWithTag( "Player" );

		if (Player != null) {
			Vector3 playerPos = Player.transform.position;
			Vector3 objectPos = gameObject.transform.position;
			if ( Vector3.Distance( playerPos, objectPos ) < LineofSight && gameObject.activeSelf)
			{
				Instantiate (bullet, transform.position, transform.rotation);
			}
		}

	}
}
