using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	public float speed;
	public float strikeDistance;
	public float remoteDistance;

	private GameObject Player;
	private GameObject Snek;
	private bool strike;
	private Vector3 playerPosition;

	// UNUSED
	void Start () {
		Snek = this.gameObject;
	}
	

	void FixedUpdate () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		playerPosition = Player.transform.position;
		if (strike) {
			Snek.transform.position = playerPosition.MoveTowards(
		}
	}
}
