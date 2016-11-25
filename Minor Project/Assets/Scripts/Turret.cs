using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	public float triggertime = 2f;
	public float repeatrate = 1f;
	public float LineofSight = 10f;
	public float rotationSpeed = 3.0f;
	public GameObject bullet;

	private Vector3 startPos;
	private Quaternion startRot;
	private GameObject Player;

	// Use this for initialization
	void Start () 
	{
		//Start position + rotation of the turret
		startPos = gameObject.transform.position;
		startRot = gameObject.transform.rotation;

		// Call BulletTrigger every so often
		InvokeRepeating("BulletTrigger",triggertime,repeatrate);
	}
	

	void Update () {
		// Aim at the player

		Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;;


		if ( Vector3.Distance( playerPos, objectPos ) < LineofSight )
		{
			gameObject.transform.rotation = Quaternion.Slerp( objectRot, Quaternion.LookRotation( playerPos - objectPos ), rotationSpeed * Time.deltaTime );
		}
		// Return to start position if no player is found
		else
		{
			gameObject.transform.rotation = Quaternion.Slerp( objectRot, startRot, rotationSpeed * Time.deltaTime );
		}
	}

	public void BulletTrigger(){
		// Fire a bullet

		Player = GameObject.FindGameObjectWithTag( "Player" );
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;

		if ( Vector3.Distance( playerPos, objectPos ) < LineofSight && gameObject.activeSelf)
		{
			Instantiate (bullet, transform.position, transform.rotation);
		}
	}
}
