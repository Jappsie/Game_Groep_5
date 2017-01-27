using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	public Vector3 center;
	public Vector3 direction;
    public float movementSpeed;
    private float cycle;

	// Use this for initialization
	void Start () {
		center = new Vector3 (0, 0, 0);
		direction = Vector3.left;
		movementSpeed = 4f;
        cycle = movementSpeed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(center, direction, cycle);
        transform.LookAt(center);
	}
}
