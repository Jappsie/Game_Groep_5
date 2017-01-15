﻿using UnityEngine;
using System.Collections;

public class Eggs : MonoBehaviour {
	public float thrust = 350f;
	public float spawntime = 3.0f;
	public GameObject BabyBee;

	// Use this for initialization
	void Start () {
		GameObject Player = GameObject.FindGameObjectWithTag("Player"); // Finds Player with Tag Player
		Physics.IgnoreCollision (gameObject.GetComponent<CapsuleCollider> (), Player.GetComponent<CharacterController> ());
		gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.forward * thrust);
		Invoke ("EggSpawn", spawntime);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter (Collision col){
		if (col.gameObject.tag == "Egg") {
			Physics.IgnoreCollision (gameObject.GetComponent<CapsuleCollider> (), col.gameObject.GetComponent<CapsuleCollider> ());
		
		// Destroy egg if collided with player or playerbullet
		} else if (col.gameObject.tag == "PlayerBullet" || col.gameObject.tag == "Player") {
			Destroy (this.gameObject);
		
		} else if (col.gameObject.tag == "Boss") {
			Physics.IgnoreCollision (gameObject.GetComponent<CapsuleCollider> (), col.gameObject.GetComponent<SphereCollider> ());
		}

	}

	void EggSpawn(){
		GameObject newBee = (GameObject) Instantiate (BabyBee, new Vector3(transform.position.x, transform.position.y +2f, transform.position.z), transform.rotation);
        newBee.gameObject.tag = "offspring";
        proceduralEnemyGeneration(newBee);
        Destroy (this.gameObject);
	}

    void proceduralEnemyGeneration(GameObject creature)
    {
        float color = Random.value;

        BabyBee beeParams = creature.GetComponent<BabyBee>();

        creature.transform.GetChild(1).transform.GetComponent<Renderer>().material.color = new Color(color, 0, 0);
        beeParams.BeeLife = (int)(2 * color + 1); // [1, 3]
        beeParams.moveSpeed = (int)(4 * color + 4); // [4, 8]
        beeParams.Damage = color + 1.0f; // [1, 2]
    }
}