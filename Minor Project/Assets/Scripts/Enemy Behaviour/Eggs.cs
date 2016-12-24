using UnityEngine;
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
		
		
		} else if (col.gameObject.tag == "PlayerBullet") {
			Destroy (this.gameObject);
		
		} else if (col.gameObject.tag == "Boss") {
			Physics.IgnoreCollision (gameObject.GetComponent<CapsuleCollider> (), col.gameObject.GetComponent<SphereCollider> ());
		}

	}

	void EggSpawn(){
		Instantiate (BabyBee, new Vector3(transform.position.x, transform.position.y +2f, transform.position.z), transform.rotation);
		Destroy (this.gameObject);
	}
}