using UnityEngine;
using System.Collections;

public class babySnek : MonoBehaviour {

	private GameObject papaSnek;

	void Start() {
		papaSnek = GameObject.FindGameObjectWithTag ("SnakeController");
	}
	void OnCollisionStay(Collision other) {
		if (other.gameObject.CompareTag("Crystal")) {
			Debug.Log ("Test werkt de collision???");
			papaSnek.GetComponent<HealthSystem> ().TakeDamage (0.1f);
		}
	}
}