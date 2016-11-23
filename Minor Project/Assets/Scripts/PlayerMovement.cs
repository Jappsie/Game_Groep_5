using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : HealthSystem
{

	public float speed = 10f;       // Speed variable
	public float gravity = 14.0f;    // Gravity effect
	public float jumpForce = 1.0f;  // Force of jump
	public float zweefConstant = 5.0f;	//Gravity effect during float

	private CharacterController controller;     // Controller of the movement
	private float verticalVelocity;            // Velocity regarding jump/gravity
	private Quaternion rotation;                // Align movement with camera position

	// Get reference to the CharacterController
	void Start()
	{
		controller = GetComponent<CharacterController>();
		Vector3 camera = GameObject.FindGameObjectWithTag( "MainCamera" ).transform.position;
		camera.y = gameObject.transform.position.y;
		rotation = Quaternion.LookRotation( transform.position-camera, Vector3.up );
	}

	// Make the object move
	void Update()
	{
		// Jump if not floating
		if (controller.isGrounded)
		{
			verticalVelocity = 0.0f;
			if ( Input.GetKey( KeyCode.Space ) )
			{
				verticalVelocity = jumpForce;
			}
		}
		else
		{
			if (Input.GetKey (KeyCode.Space) && verticalVelocity < 0) {
				verticalVelocity -= zweefConstant * Time.deltaTime;
			} else {
				verticalVelocity -= gravity * Time.deltaTime;
			}
		}

		// Move about in the 2D area
		float horizontalMovement = Input.GetAxis( "Horizontal" );
		float verticalMovement = Input.GetAxis( "Vertical" );

		Vector3 movement = new Vector3( speed * horizontalMovement, verticalVelocity, speed * verticalMovement );
		controller.Move( rotation * movement * Time.deltaTime );
	}

	public override void Death()
	{
		Debug.Log( "Player died" );
		//Awake();
		GameObject.Find( "SceneController" ).GetComponent<SceneManagerScript>().reset();
	}
}