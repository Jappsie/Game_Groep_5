using UnityEngine;
using System.Collections;
using System;

public class EnemyFollowing : HealthSystem
{
    public float followDistance = 10f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 3.0f;
    public float Damage = 1;

	public bool Dodges;							//true for bullet dodging enemy
	public float DodgeDistance = 1f;			//amount of distance the dodge manouvre should cover
	public float BulletDistanceTrigger = 5f;	//radius of the circle circumscribing the enemy, wherein a bullet may be detected 
	public float FoV = 120f;					//the angle of the cone projected from the front of the enemy. bullets are only detected when entering this cone
	public float MoveMultiplier = 2f;			//The multiplication factor of the 'moveSpeed' used to determine the dodging speed

    private GameObject Player;
	private GameObject Bullet;
    private Vector3 startPos;

	private float FOV;							//FoV divided by two.
	private float DodgeSpeed;					//result of movemultiplier times movespeed
	private bool dodging;						//used to run and block some of the code once a bullet fullfills certain cirteria	
	private int LeftOrRight;					//saves the result from the surrounding check
	private Vector3 current;					//curreent position of the enemy

    // Use this for initialization
    void Start()
    {
        startPos = gameObject.transform.position;
		DodgeSpeed = MoveMultiplier * moveSpeed;
		FOV = FoV / 2f;
		dodging = false;

    }

    // Follow a 'player' if within followDistance
	protected override void Update()			//Now overrides the Update of HealthSystem to check y pos
	{
		base.Update ();							//Call to Update of Parent

		Player = GameObject.FindGameObjectsWithTag ("Player") [0];
		Vector3 playerPos = Player.transform.position;

		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;

		//when the enemy can dodge, check for a player bullet on the field 
		if (Dodges && GameObject.FindGameObjectsWithTag ("Attack").Length > 0 && !dodging) {
			
			Bullet = GameObject.FindGameObjectsWithTag ("Attack") [0];
			Vector3 bulletPos = Bullet.transform.position;
			Vector3 Direction = - this.transform.position + bulletPos;

			//if this bullet gets close enough, calculate the dot product the enemy/bullet vector and the enemiy's forward vector
			//also determin if the bullet enters the FoV cone
			if (Vector3.Distance (bulletPos, objectPos) < BulletDistanceTrigger) {
				float AtUs = Vector3.Dot (this.transform.forward, Direction);
				float Angle = Vector3.Angle (Direction, this.transform.forward);
				Debug.Log ("Bullet near! " + Time.time);

				//if the dotproduct is possitive and the angle small enough, check enemy surroundings for obstacles
				if (Angle <= FOV && AtUs >= 0) {
					Ray left = new Ray (transform.position, transform.right * -1f);
					Ray right = new Ray (transform.position, transform.right);
					Debug.Log ("IT'S COMING RIGHT AT US " + Time.time);

					RaycastHit LHit;
					RaycastHit RHit;

					float LDistance = 0f;
					float RDistance = 0f;

					//if there is an obstacle to the left, save the distnace to it
					if (Physics.Raycast (left, out LHit)) {
						LDistance = LHit.distance;
					}
					//if there is an obstacle to the right, save the distnace to it
					if (Physics.Raycast (right, out RHit)) {
						RDistance = RHit.distance;
					}
					//determin which direction has the most dodging room, and put this in an int
					//then set the dodgin boolean to true
					if (LDistance > RDistance) {
						Debug.Log ("LEFT! " + Time.time);
						LeftOrRight = 0;
						current = this.transform.position;
						dodging = true;
					} else {
						Debug.Log ("RIGHT! " + Time.time);
						LeftOrRight = 1;
						current = this.transform.position;
						dodging = true;
					}
				}
			}
		}
		//use the direction determined to dodge in, till a set distance has been reached.
		if (dodging) {
			switch (LeftOrRight) {
			case 0:
				this.transform.position += -1f * this.transform.right * DodgeSpeed * Time.deltaTime;
				break;
			case 1:
				this.transform.position += this.transform.right * DodgeSpeed * Time.deltaTime;
				break;
			}
			if (Vector3.Distance (current, this.transform.position) >= DodgeDistance) {
				dodging = false;
			}
				
		} else if (Vector3.Distance (playerPos, objectPos) < followDistance && !dodging) {
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (playerPos - objectPos), rotationSpeed * Time.deltaTime);
		} else if(!dodging){
			gameObject.transform.rotation = Quaternion.Slerp (objectRot, Quaternion.LookRotation (startPos - objectPos), rotationSpeed * Time.deltaTime);
		}

		if (!dodging) {
			gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
		}
	}


	private void OnCollisionStay( Collision collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) )
        {
            Debug.Log( collision.gameObject.name + " Got Damaged");
            collision.gameObject.SendMessage( "TakeDamage", Damage );
        }
    }

    private void OnTriggerStay( Collider other )
    {
        Debug.Log( other.gameObject.name + " Got Triggered" );
    }

    protected override void Death()
    {
        Destroy(gameObject); 
    }
}