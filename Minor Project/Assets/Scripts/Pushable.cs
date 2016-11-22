using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {

	public float Speed;
	public float Delay;
	public bool move_over_Y;
	public bool move_over_X;
	public bool move_over_Z;
	public bool GridSnap;

	public Vector3 gridSize;

	private int id;

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
	}

	void OnCollisionStay (Collision boem){

		if (boem.gameObject.tag.Equals ("Player")) {
			switch (id) {
			case 0: 
				if (move_over_X) {
					transform.Translate (Speed * Time.deltaTime, 0, 0, Space.World);
				}
				break;
			case 1:
				if (move_over_Y) {
					transform.Translate (0, Speed * Time.deltaTime, 0, Space.World);
				}
				break;
			case 2:
				if (move_over_Z) {
					transform.Translate (0, 0, Speed * Time.deltaTime, Space.World);
				}
				break;
			}
		}
	}

	void OnCollisionExit (Collision boem){

		if (boem.gameObject.tag.Equals ("Player") && GridSnap) {

			Vector3 currentPos = transform.position;
			Vector3 move = new Vector3 ((Mathf.Round (currentPos.x / gridSize.x)), currentPos.y, Mathf.Round (currentPos.z / gridSize.z));
			transform.position = move;

		}
	}
}
