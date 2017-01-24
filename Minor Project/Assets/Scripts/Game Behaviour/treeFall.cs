using UnityEngine;
using System.Collections;

public class treeFall : MonoBehaviour {
	Animator animator;
    AudioSource audio;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
        audio = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider col){
		if (col.CompareTag("saw")) {
			animator.Play ("FallingTree");
            audio.Play();
            Destroy(this);
		}
	} 
}
