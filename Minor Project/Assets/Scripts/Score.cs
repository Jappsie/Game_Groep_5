using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public float tijd;
	public int deaths;
	public GameObject player;

	// Use this for initialization
	void Start () {
		tijd = 0f;
		deaths = 0;
	}

	// Update is called once per frame
	void Update () {
		tijd = Time.time;
		Debug.Log (tijd);
	}
}
