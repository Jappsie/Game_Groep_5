using UnityEngine;
using System.Collections;

public class treeFall : MonoBehaviour {
	Animator animator;
	GameObject player;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
        audio = GetComponent<AudioSource>();
	}
		
	public void ThreeCut(){
			animator.Play ("FallingTree");
            audio.Play();
			audio.enabled = false;
            //Destroy(audio);
	}
}
