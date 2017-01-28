using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	public Camera mc;
    public Vector3 center;
    public Vector3 direction;
    public float movementSpeed;
	private Light light;
    private float cycle;
	public float posX;

	public Color dayBack;
	public Color nightBack;

	public bool newDay;
	public Color curColor;

	// Use this for initialization
	void Start () {
		mc = GameObject.Find("Main Camera").GetComponent<Camera>();
		newDay = false;

		//dayBack = new Color(206, 241, 255);
		//nightBack = new Color(23, 20, 62);

		light = this.GetComponent<Light>();
		center = new Vector3(0,0,0);
        cycle = movementSpeed * Time.deltaTime;
		direction = Vector3.left;
	}
	
	// Update is called once per frame
	void Update () {
        
		posX = transform.eulerAngles.x;

		if ((posX < 360f && posX > 270f)  && light.intensity > 0f) {
			light.intensity += -0.001f;
			mc.backgroundColor = Color.Lerp(nightBack, mc.backgroundColor, 0.99f);
			
		}			
		if ((posX > 0 && posX < 90f) && light.intensity < 1.0f) {
			light.intensity += 0.001f;
			mc.backgroundColor = Color.Lerp(dayBack, mc.backgroundColor, 0.99f);
		}
		transform.RotateAround(center, direction, cycle);
        transform.LookAt(center);
	}
}
