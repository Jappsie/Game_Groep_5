using UnityEngine;
using System.Collections;

public class animationBullet : MonoBehaviour {
	Animator animator;
	public GameObject[] toDisable;

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
			for (int i = 0; i < toDisable.Length; i++) {
				toDisable [i].gameObject.SetActive (false);
			}
			animator.Play ("stone rolling");
		}
		if (col.CompareTag ("Disabler")) {
			Debug.Log ("TEST");
			for (int i = 0; i < toDisable.Length; i++) {
				toDisable [i].gameObject.SetActive (false);
			}
		}
	} 
}
