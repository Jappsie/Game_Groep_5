using UnityEngine;
using System.Collections;

public class SawDroppingAnim : MonoBehaviour {

    // dropping animatie
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
        anim.Play("SawDropping");
	}

    void OnTriggerEnter(Collider col)
    {
        // if collision with player
        if (col.gameObject.tag == "Player")
        {
            // align movement with world coordinates
            anim.applyRootMotion = true;
            // and destroy this script
            Destroy(this);
        }
    }
}
