using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float speed;
	public float instantness;
	public float jumpiness;

	private bool grounded;
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision hit){
		if (hit.gameObject.tag=="Ground"){
			grounded=true;
		}
	}
	void OnCollisionExit(Collision hit){
		if (hit.gameObject.tag == "Ground") {
			grounded = false;
		}
	}

	void FixedUpdate () {

		float horizontalMovement = Input.GetAxis("Horizontal");        
		float verticalMovement = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(horizontalMovement, 0.0f, verticalMovement);

		if (movement != Vector3.zero) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (movement), instantness);
		}

		if (Input.GetKeyDown ("space") && grounded) {

			rb.AddForce(Vector3.up * jumpiness);

		}



		transform.Translate (movement * speed * Time.deltaTime, Space.World);
	}


}