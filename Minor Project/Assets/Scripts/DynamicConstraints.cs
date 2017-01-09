using UnityEngine;
using System.Collections;

public class DynamicConstraints : MonoBehaviour {

	public bool move_over_Y;
	public bool move_over_X;
	public bool move_over_Z;

	private Rigidbody rb;
	private int id;


	void Start () {
		rb = GetComponent <Rigidbody> ();
	
	}
	//When player collides with pushable object, the direction of movement is determined
	void OnCollisionEnter (Collision boem){
		if (boem.gameObject.tag.Equals ("Player")) {

			Vector3 incomming = boem.relativeVelocity;

			float temp = incomming.x;
			id = 0;

			if (incomming.y > temp) {
				temp = incomming.y;
				id = 1;
			} else if (incomming.z > temp) {
				id = 2;
			}
		}
		//Contraints are set for the pushable object, according to the determined direction of movement
		switch (id) {
		case 0:
			rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			break;
		case 1:
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
			break;
		case 2:
			rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
			break;
		}
	}
	//on collision end the contraints are reset
	void OnCollisionExit (Collision boem){
		if (boem.gameObject.tag.Equals ("Player")){
			
		rb.constraints = RigidbodyConstraints.None;

		}
	}
}
