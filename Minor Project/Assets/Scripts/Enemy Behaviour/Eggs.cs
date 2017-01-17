using UnityEngine;
using System.Collections;

public class Eggs : MonoBehaviour {
	public float thrust = 350f;
	public float spawntime = 3.0f;
	public GameObject BabyBee;

	// Use this for initialization
	void Start () {
		GameObject Player = GameObject.FindGameObjectWithTag("Player"); // Finds Player with Tag Player
		//Physics.IgnoreCollision (gameObject.GetComponent<CapsuleCollider> (), Player.GetComponent<CharacterController> ());
		gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.forward * thrust);
		Invoke ("EggSpawn", spawntime);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter (Collision col){
		Debug.Log (col.gameObject.name);
		if (col.gameObject.CompareTag("Egg")) {
			Physics.IgnoreCollision (gameObject.GetComponent<CapsuleCollider> (), col.gameObject.GetComponent<CapsuleCollider> ());
		
		// Destroy egg if collided with player or playerbullet
		} else if (col.gameObject.CompareTag("PlayerBullet")) {
			Destroy (this.gameObject);
		
		} else if (col.gameObject.CompareTag("Boss")) {
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
        beeParams.moveSpeed = (int)(8 * color + 6); // [6, 14]
        beeParams.Damage = color + 1.0f; // [1, 2]
    }
}