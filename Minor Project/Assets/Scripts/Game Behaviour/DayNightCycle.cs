using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

    public Vector3 center = new Vector3(0, 0, 0);
    public Vector3 direction = Vector3.right;
    public float movementSpeed;
    private float cycle;

	// Use this for initialization
	void Start () {
        cycle = movementSpeed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(center, direction, cycle);
        transform.LookAt(center);
	}
}
