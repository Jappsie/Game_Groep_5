using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	private float speed = 0.2f;
	private bool crawling = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (Vector3.up * Time.deltaTime * speed);
	}
}
