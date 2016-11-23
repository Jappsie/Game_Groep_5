using UnityEngine;
using System.Collections;

public class Bullit : MonoBehaviour {
	public float firespeed = 7.0f;
	private Vector3 direction;
	private GameObject Player;

	//wordt gebruikt om directie van de kogel te vinden
	void Start () {

		Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
		direction = (Player.transform.position - transform.position).normalized;
	}
	
    //Kogel snelheid
	void Update () {

		transform.position += direction * (firespeed * Time.deltaTime);
		Destroy (this.gameObject, 3.0f);
	}
}
