﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : HealthSystem
{

	public float speed = 10f;       // Speed variable
    public float rotationSpeed = 1;
	public float gravity = 14.0f;    // Gravity effect
	public float jumpForce = 1.0f;  // Force of jump
	public float zweefConstant = -5.0f;	//Gravity effect during float
	public static bool  AbleShoot = true; // Bool to check if player is able to shoot
	public GameObject Playerbullet; // Bullet Player uses
	public Vector3 MousePosition; // Position mouseRaycast on plane

	private CharacterController controller;     // Controller of the movement
	private float verticalVelocity;            // Velocity regarding jump/gravity
    private Quaternion axisRotation;                // Align movement with camera position
	private Vector3 movement;



	// Get reference to the CharacterController
	void Start()
	{
		controller = GetComponent<CharacterController>();
		Vector3 camera = GameObject.FindGameObjectWithTag( "MainCamera" ).transform.position;
		camera.y = gameObject.transform.position.y;
        axisRotation = Quaternion.LookRotation( gameObject.transform.position - camera, Vector3.up );
	}

	// Make the object move
	protected override void Update()		//Now overrides the Update of HealthSystem to check y position
	{
		base.Update ();						//Call to Update of Parent

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
				verticalVelocity = zweefConstant;		//Constant falling speed when hovering
			} else {
				verticalVelocity -= gravity * Time.deltaTime;
			}
		}

		// Move about in the 2D area
		float horizontalMovement = Input.GetAxis( "Horizontal" );
		float verticalMovement = Input.GetAxis( "Vertical" );
        movement = axisRotation * new Vector3( speed * horizontalMovement, verticalVelocity, speed * verticalMovement );

		// Stops the player from moving when he has shot a bullet
		if (AbleShoot == true) {
			controller.Move (movement * Time.deltaTime); 
		}
		else {
		  movement = axisRotation * new Vector3( 0, verticalVelocity, 0 );	
			controller.Move (movement * Time.deltaTime);
		}


        movement.y = 0;
        Quaternion rotation = Quaternion.LookRotation( Vector3.right, Vector3.up );	// Rotate player object to correct orientation
        if ( !(movement.magnitude == 0) )
        {
            rotation = rotation * Quaternion.LookRotation( movement, Vector3.up );
            gameObject.transform.rotation = Quaternion.Slerp( gameObject.transform.rotation, rotation, rotationSpeed * Time.deltaTime );

        }

		//Finds mouse position in the world on a plane at height of the main caracter
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane Hitplane = new Plane(new Vector3(0,1,0), transform.position);
		float distance = 0f;

		if (Hitplane.Raycast (ray, out distance)) {
			MousePosition = ray.GetPoint (distance);
			transform.LookAt (MousePosition);	
		}

		//Left mouse click to fire a bullet
		if (Input.GetKey (KeyCode.Mouse0) && AbleShoot == true) {
			AbleShoot = false;
			Instantiate (Playerbullet, transform.position, transform.rotation);


		
		
		}
	}

	private void BulletTrigger(){
		

	}



	// Apply force on collision
	private void OnControllerColliderHit( ControllerColliderHit hit) {
		Rigidbody body = hit.gameObject.GetComponent<Rigidbody> ();
		if (body != null) {
			hit.rigidbody.AddForce(50/hit.rigidbody.mass * movement);
		}
	}

	// reset the scene
	protected override void Death()		//Death is now protected
	{
		GameObject.Find( "SceneController" ).GetComponent<SceneManagerScript>().reset();
	}



}