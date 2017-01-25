using UnityEngine;
using System.Collections;

public class BulletSound : MonoBehaviour {

    public AudioSource[] sound;

	// Use this for initialization
	void Start () {
        sound = GetComponents<AudioSource>();
        sound[0].Play();
	}

	// Play other sound when bullet is destroyed
	void OnDestroy () {
        AudioSource.PlayClipAtPoint(sound[1].clip, transform.position, 1.0f);
	}
}
