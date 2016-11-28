using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {
	private Vector3 direction; // direction of the bullet

	public float bulletspeed = 7.0f; // Speed at wich the bullet moves in the direction
	public float DestroyTime = 3.0f; // Time before bullet destroys itself
	private PlayerMovement Playermovement; // Used to change the bool AbleShoot


	void Start () {
		GameObject Player = GameObject.FindGameObjectWithTag("Player"); // Finds Player with Tag Player
	    Playermovement = Player.GetComponent<PlayerMovement>(); // Gets the script PlayerMovemennt
		direction = (Playermovement.MousePosition - Player.transform.position).normalized;// Difference between mousePosition and Playey
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * (bulletspeed * Time.deltaTime);

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
