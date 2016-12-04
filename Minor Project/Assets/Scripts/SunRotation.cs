using UnityEngine;
using System.Collections;

public class SunRotation : MonoBehaviour {
	public float speed = 2f;			//Speed of continuous rotation
	public bool ToRotation;				//Rotate the source to a preset rotation
	public Vector3 DesiredRotation;		//The desired, preset rotation 
	public float rate = 0.05f;			//rotation rate for Slerp, clamped to [0, 1]

	private Quaternion towards;

	void Start() {
		towards = Quaternion.FromToRotation (this.transform.rotation.eulerAngles, DesiredRotation);	//set DesiredRotation to Quaternion

	}
	// Update is called once per frame
	void FixedUpdate () {
		if (!ToRotation) {										//if no desired rotation, rotate at 'speed' velocity
			Vector3 rotation = new Vector3 (0f, speed, 0f);
			this.transform.Rotate (rotation * Time.deltaTime, Space.World);
	
		} else if (ToRotation) {								//if desired rotation, move to set rotation with 'rate' speed
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, towards, rate); 
		
		}
	}
}
