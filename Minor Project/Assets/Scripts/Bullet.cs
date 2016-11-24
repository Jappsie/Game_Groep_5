using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float firespeed = 7.0f;
	public float damage = 1f;
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

	private void OnTriggerEnter( Collider collision )
	{
		if ( collision.gameObject.CompareTag( "Player" ) )
		{
			Debug.Log( collision.gameObject.name + " Got Damaged");
			collision.gameObject.SendMessage( "TakeDamage", damage );
		}
		if ( !collision.gameObject.CompareTag( "Turret" )) {
		 Destroy (gameObject);
		}
	}
}
