using UnityEngine;
using System.Collections;

public class EnemyFollowing : MonoBehaviour {

	public float volgafstand = 10f;
	public float movespeed = 2.0f;
	public float Rotspeed = 3.0f;
	private GameObject Player;
	private Vector3 initielepositie;

	// Use this for initialization
	void Start () {
		initielepositie = transform.transform.position;
	}
	void Update() {
		Player = GameObject.FindGameObjectsWithTag ("Player")[0];
		Vector3 pos1 = Player.transform.position; //Positie main caracter in worldspace
		Vector3 pos2 = gameObject.transform.position;

		if (Vector3.Distance (pos1, pos2) < volgafstand) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (pos1 - pos2), Rotspeed * Time.deltaTime);
			transform.position += transform.forward * movespeed * Time.deltaTime;
		} else {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (initielepositie - pos2), Rotspeed * Time.deltaTime);
	    	transform.position += transform.forward * movespeed * Time.deltaTime;
		}
	
	}
}