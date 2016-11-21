using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speed = 10f;     // Snelheid variabele
	public bool isDead;		// isDead tag
	public float gravity= 14.0f;
	public float jumpforce= 10.0f;

	private CharacterController controller;
	private float Verticalverlocity;

    // Vind het physics engine object
    void Start()
    {
		controller = GetComponent<CharacterController> ();
    }
	
    // Update de forces iedere update dat er forces gebruikt worden
	void Update () {
		if(controller.isGrounded) {
			Verticalverlocity = 0.0f;
			if(Input.GetKey(KeyCode.Space)) {
				Verticalverlocity = jumpforce;
			}
		} else {
			Verticalverlocity -= gravity*Time.deltaTime;
		}

		float horizontalMovement = Input.GetAxis("Horizontal");        
		float verticalMovement = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(speed*horizontalMovement, Verticalverlocity, speed*verticalMovement);

		controller.Move (movement * Time.deltaTime);

	}  

}