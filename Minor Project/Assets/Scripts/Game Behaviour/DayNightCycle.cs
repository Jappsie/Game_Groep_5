using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	public Camera mc;

	// Rotation parameters

    public Vector3 center;
    public Vector3 direction;
    public float movementSpeed;
	private float cycle;
	public float posX;

	// sun
	private Light light;
	// moon
	private Light light2;
	// moonlight brightness	
	public float moonlightBrightness;
	// camera backgrounds for day and night
	public Color dayBack;
	public Color nightBack;
	public Color curColor;

	// Use this for initialization
	void Start () {
		//dayBack = new Color(206, 241, 255);
		//nightBack = new Color(23, 20, 62);
		RenderSettings.ambientIntensity = 1.0f;
		mc = GameObject.Find("Main Camera").GetComponent<Camera>();
		light = this.GetComponent<Light>();
		light2 = GameObject.Find("Moonlight").GetComponent<Light>();
		light2.intensity = 0f;
		center = new Vector3(0,0,0);
        cycle = movementSpeed * Time.deltaTime;
		direction = Vector3.left;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!mc) { Start(); return; };

		// current angular position of the sun
		posX = transform.eulerAngles.x;

		// Case for sunset
		if ((posX > 270f && posX < 360f)  && light.intensity > 0f) {
			// decrease sun intensity
			light.intensity -= 0.01f;
			// make transition to night sky
			mc.backgroundColor = Color.Lerp(nightBack, mc.backgroundColor, 0.9f);
			if (light.intensity < 0.2f && light2.intensity < moonlightBrightness) {
				light2.intensity += 0.01f;
				if (RenderSettings.ambientIntensity > 0.2f)
					RenderSettings.ambientIntensity -= 0.1f;
			}
			
		}
		// Case for sunrise			
		if ((posX > 0 && posX < 90f) && light.intensity < 0.9f) {
			// increase sun intensity
			light.intensity += 0.01f;
			// make transition to daylight
			mc.backgroundColor = Color.Lerp(dayBack, mc.backgroundColor, 0.9f);
			if (light.intensity > 0.9 && light2.intensity > 0) {
				light2.intensity -= 0.01f;
				if (RenderSettings.ambientIntensity < 1.0f)
					RenderSettings.ambientIntensity += 0.1f;
			}
		}
		// always move around the center and point in the direction of it
		transform.RotateAround(center, direction, cycle);
        transform.LookAt(center);
	}
}
