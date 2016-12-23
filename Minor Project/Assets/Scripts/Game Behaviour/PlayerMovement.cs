using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : HealthSystem
{
    public float strength = 50f;                // Pushing strength
    public float speed = 10f;                   // Movement speed
    public float rotationSpeed = 1;             // Rotation speed 
    public float gravity = 14.0f;               // Gravity effect
    public float jumpForce = 1.0f;              // Force of jump
    public float zweefConstant = 5.0f;          // Gravity effect during float
    public static bool AbleShoot = true;        // Bool to check if player is able to shoot
    public Vector3 bulletSpawn;                 // Position where the bullit will spawn
    public GameObject Playerbullet;             // Bullet Player uses
    public float MinMomentum = 1f;              // Minimum momentum
    public float MaxMomentum = 10f;             // Maxmomentum
    public float Momentumcharge = 5f;            // Used for scaling the momentum increase

    [HideInInspector]
    public Vector3 MousePosition;               // Position mouseRaycast on plane
    [HideInInspector]
    public float Momentum;                      // Momentum on the playerbullet

    private CharacterController controller;     // Controller of the movement
    private float verticalVelocity;             // Velocity regarding jump/gravity
    private Quaternion axisRotation;            // Align movement with camera position
    private Vector3 movement;                   // Movement vector
    private GameObject cam;                     // Camera object
    private bool Mouserelease;                  // Track mouse clicking



    // Get reference to the CharacterController
    void Start()
    {
        Debug.Log( "Player start" );
        controller = GetComponent<CharacterController>();
        GameObject[] cameras = GameObject.FindGameObjectsWithTag( "MainCamera" );
        cam = cameras[ cameras.Length - 1 ];
        Vector3 camera = cam.transform.position;
        camera.y = gameObject.transform.position.y;
        axisRotation = Quaternion.LookRotation( gameObject.transform.position - camera, Vector3.up );
    }

    // Make the object move
    protected override void Update()            //Now overrides the Update of HealthSystem to check y position
    {
        base.Update();                          //Call to Update of Parent

        // Jump if not floating
        if ( controller.isGrounded )
        {
            verticalVelocity = 0.0f;
            if ( Input.GetKey( KeyCode.Space ) )
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // If holding space and falling glide
            if ( Input.GetKey( KeyCode.Space ) && verticalVelocity <= 0 )
            {
                verticalVelocity = -zweefConstant * Time.deltaTime;     //Constant falling speed when hovering
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }

        // Move about in the 2D area
        float horizontalMovement = Input.GetAxis( "Horizontal" );
        float verticalMovement = Input.GetAxis( "Vertical" );
        movement = axisRotation * new Vector3( speed * horizontalMovement, verticalVelocity, speed * verticalMovement );

        // Stops the player from moving when he has shot a bullet
        if ( AbleShoot == true )
        {
            controller.Move( movement * Time.deltaTime );
        }
        else
        {
            movement = axisRotation * new Vector3( 0, verticalVelocity, 0 );
            controller.Move( movement * Time.deltaTime );
        }

        //Finds mouse position in the world on a plane at height of the main character
        Camera camera = cam.GetComponent<Camera>();
        var ray = camera.ScreenPointToRay( Input.mousePosition );
        Plane Hitplane = new Plane( new Vector3( 0, 1, 0 ), transform.position );

        float distance;
        if ( Hitplane.Raycast( ray, out distance ) )
        {
            MousePosition = ray.GetPoint( distance );
            transform.LookAt( MousePosition );
        }
        
        // Counts how many seconds the left mouse butten is hold down
        if ( Input.GetMouseButton( 0 ) && AbleShoot == true )
        {
            Momentum += Momentumcharge * Time.deltaTime;
            Mouserelease = true;
            Momentum = Mathf.Clamp( Momentum, MinMomentum, MaxMomentum );
            //Debug.Log (Momentum);

        }
        // Shoots bullet when left mouse click is released
        if ( Input.GetMouseButton( 0 ) == false && Mouserelease == true )
        {
            Instantiate( Playerbullet, transform.position + transform.rotation * bulletSpawn, transform.rotation );
            //Momentum = Momentum * Momentumscale;
            AbleShoot = false;
            Mouserelease = false;
        }
    }

    // Apply force on collision with Constrained Objects
    private void OnControllerColliderHit( ControllerColliderHit hit )
    {
        Rigidbody body = hit.gameObject.GetComponent<Rigidbody>();

		if (body != null && hit.gameObject.CompareTag ("Constrained")) {
			Debug.Log (movement);
			if (Mathf.Abs (movement.x) > Mathf.Abs (movement.z)) {
				body.AddForce (strength / body.mass * new Vector3 (1f, 0, 0));
			} else if (Mathf.Abs (movement.z) > Mathf.Abs (movement.x)) {
				body.AddForce (strength / body.mass * new Vector3 (0, 0, 1f));
			}
		} else if (body != null) {
			hit.rigidbody.AddForce (strength / hit.rigidbody.mass * movement);
		}

    }

    // Reset the scene when player dies
    protected override void Death()     //Death is now protected
    {
        //GameObject.Find( "SceneController" ).GetComponent<SceneManagerScript>().resetOnDeath();
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<SceneManagerScript>().resetOnDeath();
    }

}