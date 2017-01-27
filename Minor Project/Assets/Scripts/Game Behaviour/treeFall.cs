using UnityEngine;
using System.Collections;

public class treeFall : MonoBehaviour {
	Animator animator;
	GameObject player;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
		
	public void ThreeCut(){
			animator.Play ("FallingTree");
	}
}
