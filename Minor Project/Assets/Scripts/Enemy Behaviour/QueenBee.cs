using UnityEngine;
using System.Collections;

public class QueenBee : MonoBehaviour {
	public float speed = 1.0f;
	public int PingPongRange = 6;

	private Vector3 pos1;
	private Vector3 pos2;


	// Use this for initialization
	void Start () {
		pos1 = new Vector3 (transform.position.x - PingPongRange, transform.position.y, transform.position.z);
		pos2 = new Vector3 (transform.position.x + PingPongRange, transform.position.y, transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
	}
}
