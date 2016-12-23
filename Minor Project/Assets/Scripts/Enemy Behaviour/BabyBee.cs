using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BabyBee : MonoBehaviour
{

	public float rotationSpeed = 3.0f; 
	public float moveSpeed = 2.0f;
	public int BeeLife = 1;
	public float Damage=1f; //Damage by babybee on player


	private GameObject Player; // maincharacter
	private Vector3 playerPos;
	private float ypos = 2;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectsWithTag ("Player") [0];
		playerPos = Player.transform.position;
	}
	
	// Update is called once per frame
	public void Update()	
	{
		// Baby folows Player but not into the air
		if (Player.GetComponent<CharacterController> ().isGrounded) {
			ypos = Player.transform.position.y + 1f;
		}
		playerPos = new Vector3 (Player.transform.position.x, ypos, Player.transform.position.z);
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;
		gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (playerPos - objectPos), rotationSpeed * Time.deltaTime);
		gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
	}

	// checks triggerenter with player and playerbullet
	private void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Player")) {
			gameObject.SendMessage( "TakeDamage", Damage );
		}

		if (col.gameObject.CompareTag("PlayerBullet")) {
			BeeLife -= 1;
			Destroy (col.gameObject);
			if (BeeLife == 0) {
				Destroy (this.gameObject);
			}
		}
	}

}
