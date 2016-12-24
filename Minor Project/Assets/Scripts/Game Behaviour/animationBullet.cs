using UnityEngine;
using System.Collections;

public class animationBullet : MonoBehaviour {
	Animator animator;
	// Use this for initialization
	void Start () {
		Debug.Log ("Started");
		animator = GetComponent<Animator> ();
	}
	
	void onCollisionEnter(Collision other) {
		Debug.Log ("cTriggered");
		animator.SetTrigger ("New Trigger");
	}

	private void OnTriggerEnter(Collider col){
		if (col.CompareTag("TurretBullet")) {
			Debug.Log ("Triggered");
			animator.Play ("stone rolling");
		}
	} 
}
