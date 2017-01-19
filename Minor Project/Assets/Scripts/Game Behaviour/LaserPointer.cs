using UnityEngine;
using System.Collections;

public class LaserPointer : MonoBehaviour {

	LineRenderer line;
	Vector3[] positions = new Vector3[2];
	Color kleur;
	Ray ray;

	// Use this for initialization
	void Start () {
		positions[0] = gameObject.transform.position;
		positions[1] = gameObject.transform.forward * 100;
		line = gameObject.AddComponent<LineRenderer> ();
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetWidth (0.1f, 0.1f);
		kleur = Color.red;
		kleur.a = 0.3f;
		line.SetColors (kleur, kleur);
	}
	
	// Update is called once per frame
	void Update () {
		positions [0] = gameObject.transform.position;
		ray = new Ray (positions [0], gameObject.transform.forward);
		RaycastHit raycasthit;
		if (Physics.Raycast (ray, out raycasthit, 1000f)) {
			positions [1] = raycasthit.point;
			line.SetPositions (positions);
			line.enabled = true;
		} else {
			line.enabled = false;
		}
	}
}
