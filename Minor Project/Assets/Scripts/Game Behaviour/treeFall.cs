using UnityEngine;
using System.Collections;

public class treeFall : MonoBehaviour {
	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}

	private void OnTriggerEnter(Collider col){
		if (col.CompareTag("saw")) {
			Debug.Log ("Going down!");
			animator.Play ("FallingTree");
		}
	} 
}
