using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {
	public GameObject Enemy;
	public int EggLife = 1;
	public float thrust = 350f;

	// Use this for initialization
	void Start () {
		Invoke ("EnemyBirth", 4);
		this.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.back * thrust);
	}
		
	
	//Checks if the Egg is hit by the player bullet
	private void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("PlayerBullet")) {
			EggLife -= 1;
			Destroy (col.gameObject);
		}
		if (EggLife == 0) {
			Destroy (this.gameObject);
		}
	}

	// Instantiates a enemy
	void EnemyBirth(){
		Instantiate (Enemy, transform.position, transform.rotation);
		Destroy (this.gameObject);
	}
}
