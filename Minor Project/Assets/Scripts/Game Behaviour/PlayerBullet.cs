using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

	public float bulletspeed = 7.0f; // Speed at wich the bullet moves in the direction
	public float DestroyTime = 3.0f; // Time before bullet destroys itself

	private PlayerMovement Playermovement; // Used to change the bool AbleShoot
	private Rigidbody bullet; // Used  for adding the force on the playerbullet
    private Vector3 direction; // direction of the bullet



    void Start () {
		GameObject Player = GameObject.FindGameObjectWithTag("Player"); // Finds Player with Tag Player
	    Playermovement = Player.GetComponent<PlayerMovement>(); // Gets the script PlayerMovement
		direction = (Playermovement.MousePosition - Player.transform.position).normalized;// Difference between mousePosition and Player
		bullet = this.GetComponent<Rigidbody>();
		bullet.AddForce (direction*Playermovement.Momentum, ForceMode.Impulse);//Adds force on Bullet
		Debug.Log (Playermovement.Momentum);
		Playermovement.Momentum = 1; //Sets the momentum in the playermovement script back to 1
		Physics.IgnoreCollision(bullet.GetComponent<BoxCollider>(),Player.GetComponent<CharacterController>());
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position += direction * (bulletspeed * Time.deltaTime);

		Destroy (this.gameObject, DestroyTime);


	//You can destroy the bullet with right mouseclick
	if (Input.GetKey (KeyCode.Mouse1)) {
		Destroy (this.gameObject);
		}
	}

	//Changes Bool Ableshoot from Playermovement
	void OnDestroy(){
		PlayerMovement.AbleShoot = true; 
	}
}
