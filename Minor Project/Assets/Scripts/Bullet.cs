using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float firespeed = 7.0f;
	private Vector3 direction;
	private GameObject Player;

	// Find the player and get the direction
	void Start () {

		Player = GameObject.FindGameObjectWithTag( "Player" );
		direction = (Player.transform.position - transform.position).normalized;
	}
	
    //Bullet speed
	void Update () {

		transform.position += direction * (firespeed * Time.deltaTime);
		Destroy (this.gameObject, 3.0f);
	}
}
