using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {

	public float Speed; 
	public float Delay; // not used

	public bool move_over_Y;
	public bool move_over_X;
	public bool move_over_Z;

	public bool GridMode;
	public Vector3 gridSize;

	private Rigidbody rb;
	private int id;
	private bool push;
	private Vector3 target;

	void Start () {
		push = false;
		rb = GetComponent <Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotation;

		if (GridMode) {
			rb.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	void FixedUpdate () {
		if (push) {
			this.transform.position = Vector3.MoveTowards (this.transform.position, target, Speed * Time.deltaTime);

			if (target.Equals(this.transform.position)) {
				push = false;
			}
		}
	}

	//When player collides with pushable object, the direction the object will move in is determined through relative velocity
	void OnCollisionEnter (Collision boem){
		if (boem.gameObject.tag.Equals ("Player") && !push) {

			Vector3 incomming = boem.relativeVelocity;

			float temp = incomming.x;
			id = 0;

			if (incomming.y > temp) {
				temp = incomming.y;
				id = 1;
			} else if (incomming.z > temp) {
				id = 2;
			}
			if (GridMode) {
				switch (id) {
				case 0:
					if (move_over_X) {
						target = this.transform.position + new Vector3 (gridSize.x, 0, 0);
					}
					break;
				case 1:
					if (move_over_Y) {
						target = this.transform.position + new Vector3 (0, gridSize.y, 0);
					}
					break;
				case 2:
					if (move_over_Z) {
						target = this.transform.position + new Vector3 (0, 0, gridSize.z);
					}
					break;
				}
				push = true;
			}
		}
	}
	//while colliding, movement occurs in previously determined direction
	void OnCollisionStay (Collision boem){

		if (boem.gameObject.tag.Equals ("Player") && GridMode){

		}

		else if (boem.gameObject.tag.Equals ("Player") && !GridMode) {

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
	}
	//If gridsnap is active, when the collision ends, the object snaps to the closest set grid point
	void OnCollisionExit (Collision boem){
		if (boem.gameObject.tag.Equals ("Player") && !GridMode) {

			rb.constraints = RigidbodyConstraints.FreezeRotation;

		}
	}
}

