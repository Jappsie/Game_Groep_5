using UnityEngine;
using System.Collections;

public class StoneSound : MonoBehaviour {

    AudioSource audio;
    Animator anim;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {
	    if (col.CompareTag("TurretBullet"))
        {
            audio.Play();
            Destroy(this);
        }
	}
}
