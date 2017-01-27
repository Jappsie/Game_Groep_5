using UnityEngine;
using System.Collections;

public class SawDroppingAnim : MonoBehaviour {

    // dropping animatie
    Animator anim;
    // initial orientation of the saw
    Quaternion primState;

	// Use this for initialization
	void Start () {
        primState = this.transform.rotation;
        anim = this.GetComponent<Animator>();
        anim.Play("SawDropping");
	}

    void OnTriggerEnter(Collider col)
    {
        // if collision with player
        if (col.gameObject.tag == "Player")
        {
            // make sure saw is still properly aligned if caught mid-air
            this.transform.rotation = primState;
            // align movement with world coordinates
            anim.applyRootMotion = true;
            // and destroy this script
            Destroy(this);
        }
    }
}
