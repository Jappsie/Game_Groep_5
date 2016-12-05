﻿using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	
	public float triggertime = 2f;
	public float repeatrate = 1f;
	public float LineofSight = 10f;
	public float rotationSpeed = 3.0f;
	public GameObject bullet;
	public int TurretLife = 3;

	private Vector3 startPos;
	private Quaternion startRot;
	private GameObject Player;
	private Vector3 playerPos;
	private Vector3 objectPos;
	private Quaternion objectRot;

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

		Player = GameObject.FindGameObjectWithTag("Player");
		if (Player != null) {
			playerPos = Player.transform.position;
			objectPos = gameObject.transform.position;
		    objectRot = gameObject.transform.rotation;
		}

		//Destroys Turret if Turrerlife equals zero
		if (TurretLife == 0) {
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
		if (col.gameObject.CompareTag ("TurretBullet")) {
			TurretLife -= 1;
			Destroy (col.gameObject); 
		}
	}
	


	public void BulletTrigger(){
		// Fire a bullet

		Player = GameObject.FindGameObjectWithTag( "Player" );

		if (Player != null) {
			Vector3 playerPos = Player.transform.position;
			Vector3 objectPos = gameObject.transform.position;
			Vector3 Spawnpoint = transform.GetChild (1).position;
			if ( Vector3.Distance( playerPos, objectPos ) < LineofSight && gameObject.activeSelf)
			{
				Instantiate (bullet, Spawnpoint, transform.rotation);
			}
		}

	}
}
