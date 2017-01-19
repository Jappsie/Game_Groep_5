using UnityEngine;
using System.Collections;
using System;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;

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
    public GameObject[] Playerbullet;             // Bullet Player uses
    public float MinMomentum = 1f;              // Minimum momentum
    public float MaxMomentum = 10f;             // Maxmomentum
    public float Momentumcharge = 5f;           // Used for scaling the momentum increase
	public bool Saw_Equipped;                   // Checks if Saw is equipped 

	public Image Healthbar; 
	public Image Controls; 
	public Button GETIT;



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

	public Text Timer;
	public float Tijd; 


	private bool Sawanimation = true;           // Checking ability to play animation
	private Animation anim;                     // Anamation from character component
	private bool Bullet_Equipped;  
	                             

    // Get reference to the CharacterController
    void Start()
    { 
        controller = GetComponent<CharacterController>();
        GameObject[] cameras = GameObject.FindGameObjectsWithTag( "MainCamera" );
        cam = cameras[ cameras.Length - 1 ];
        Vector3 camera = cam.transform.position;
        camera.y = gameObject.transform.position.y;
        axisRotation = Quaternion.LookRotation( gameObject.transform.position - camera, Vector3.up );
		anim = GetComponent<Animation> ();
		Bullet_Equipped = true;
		Saw_Equipped = false;
    }

    // Make the object move
    protected override void Update()            //Now overrides the Update of HealthSystem to check y position
    {
        base.Update();                          //Call to Update of Parent

        // Jump if not floating
        if ( controller.isGrounded )
        {
            verticalVelocity = 0.0f;
			if ( Input.GetKey( KeyCode.Space ) || Input.GetKey(KeyCode.JoystickButton0) )
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // If holding space and falling glide
			if ( (Input.GetKey( KeyCode.Space ) || Input.GetKey(KeyCode.JoystickButton0) ) && verticalVelocity <= 0 && zweefConstant != 0 )
            {
                verticalVelocity = -zweefConstant * Time.deltaTime;     //Constant falling speed when hovering
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
        }

        //Update healthbar
        if ( Healthbar )
        {
            Healthbar.fillAmount = CurHealth / MaxHealth;
        }
	
	
	
        // Move about in the 2D area
        float horizontalMovement = Input.GetAxis( "Horizontal" );
        float verticalMovement = Input.GetAxis( "Vertical" );
        movement = axisRotation * new Vector3( speed * horizontalMovement, verticalVelocity, speed * verticalMovement );

        // Stops the player from moving when he has shot a bullet
		if ( AbleShoot == true && !Mouserelease)
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
		if ( ( Input.GetMouseButton( 0 ) || Input.GetKey(KeyCode.JoystickButton5)) && AbleShoot == true && Bullet_Equipped == true )
        {
            Momentum += Momentumcharge * Time.deltaTime;
            Mouserelease = true;
            Momentum = Mathf.Clamp( Momentum, MinMomentum, MaxMomentum );
            Debug.Log (Momentum);

        }
        // Shoots bullet when left mouse click is released
		if ( ( Input.GetMouseButton( 0 ) || Input.GetKey(KeyCode.JoystickButton5)) == false && Mouserelease == true && Bullet_Equipped == true)
        {
            Debug.Log( gameObject );
			float MomentumRange = MaxMomentum - MinMomentum;
			if (MinMomentum <= Momentum && Momentum < MinMomentum + MomentumRange / 3.0) {
				GameObject bullet = Instantiate (Playerbullet [0], transform.position + transform.rotation * bulletSpawn, transform.rotation) as GameObject;
                bullet.GetComponent<PlayerBullet>().Player = gameObject;
			} else if (MinMomentum + MomentumRange / 3.0 <= Momentum && Momentum < MinMomentum + 2.0 * MomentumRange / 3.0) {
                GameObject bullet = Instantiate( Playerbullet [1], transform.position + transform.rotation * bulletSpawn, transform.rotation) as GameObject;
                bullet.GetComponent<PlayerBullet>().Player = gameObject;
            } else if (MinMomentum + MomentumRange * 2.0 / 3.0 <= Momentum && Momentum <= MaxMomentum) {
                GameObject bullet = Instantiate( Playerbullet[ 2 ], transform.position + transform.rotation * bulletSpawn, transform.rotation ) as GameObject;
                bullet.GetComponent<PlayerBullet>().Player = gameObject;
            }
            //Momentum = Momentum * Momentumscale;
            AbleShoot = false;
            Mouserelease = false;
        }

		// Starts rotating animation when mouse is pressed
		if (Sawanimation == true & (Input.GetMouseButton(0) || Input.GetKey(KeyCode.JoystickButton5)) & Saw_Equipped == true) {
			Sawanimation = false;
			anim.Play ("ZaagAnimatie");

		}

		if(Saw_Equipped == true & (Input.GetMouseButton(1) || Input.GetKey(KeyCode.JoystickButton4))){
			GameObject child = GameObject.FindGameObjectWithTag ("saw");
			Saw_Equipped = false;
			child.transform.parent = null;
			Bullet_Equipped = true;
		}

    }

	private void OnControllerColliderHit( ControllerColliderHit hit )
	{
			if (hit.gameObject.CompareTag("Enemy")) {
				TakeDamage(0.1f);
			}

		if (hit.gameObject.CompareTag ("Egg")) {
			if (CurHealth < MaxHealth) {
				TakeDamage (-1f);
			}
			Destroy (hit.gameObject);
		}
	}

    // Apply force on collision with Constrained Objects
//    private void OnControllerColliderHit( ControllerColliderHit hit )
//    {
//        Rigidbody body = hit.gameObject.GetComponent<Rigidbody>();
//
//		if (body.gameObject.CompareTag("Egg")) {
//			TakeDamage(0.1f);
//		}
//
//		if (body != null && hit.gameObject.CompareTag ("Constrained")) {
//			Debug.Log (movement);
//			if (Mathf.Abs (movement.x) > Mathf.Abs (movement.z)) {
//				body.AddForce (strength / body.mass * new Vector3 (1f, 0, 0));
//			} else if (Mathf.Abs (movement.z) > Mathf.Abs (movement.x)) {
//				body.AddForce (strength / body.mass * new Vector3 (0, 0, 1f));
//			}
//		} else if (body != null) {
//			hit.rigidbody.AddForce (strength / hit.rigidbody.mass * movement);
//		}
//
//    }

	// detects collision with powerups
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "saw") {
			col.transform.parent = transform;
			col.transform.localPosition = new Vector3 (0f, 3.5f, 0f);
			Bullet_Equipped = false;
			Saw_Equipped = true;
		}
	}

    // Reset the scene when player dies
    protected override void Death()     //Death is now protected
    {
        if ( GameObject.FindGameObjectWithTag( "GameManager" ) != null )
        {
            GameObject.FindGameObjectWithTag( "GameManager" ).GetComponent<GameManager>().deadPlayer( gameObject.transform.position );
        }
        GameObject.FindGameObjectWithTag( "GameController" ).GetComponent<SceneManagerScript>().resetOnDeath();
    }

	//Changes bool at the end of the animation
	public void AnimationEnded(){
		Sawanimation = true;
	}



	public void GetItClick ()
	{
		Controls.CrossFadeAlpha (0.0f, 0.1f, false);
		GETIT.gameObject.SetActive (false); 



	}
}