using UnityEngine;
using System.Collections;

public class WaterWiggler : MonoBehaviour {
	public Vector2 range = new Vector2(0.001f, 0.01f);
	public float speed = 0.004f;
	public bool waterfall = false;
	public float fallUpdateInS = 0.1f;

	private float[] randomHeights;
	private Mesh mesh;
	private float counter;


	void Start(){
		mesh = GetComponent<MeshFilter>().mesh;
		randomHeights = new float[mesh.vertices.Length];

		for (int i = 0; i < mesh.vertices.Length; i++) {
			randomHeights[i] = Random.Range(range.x, range.y);
		}


	}
	void FixedUpdate(){
		counter += Time.deltaTime;

		if (waterfall && counter > fallUpdateInS) {
			for (int i = 0; i < mesh.vertices.Length; i++) {
				randomHeights [i] = Random.Range (range.x, range.y);
			}
			mesh = GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;

			for (int i = 0; i < vertices.Length; i++) {
				vertices [i].z = randomHeights [i];
			}
			mesh.vertices = vertices;
			counter = 0f;
		}
	}

	void Update() {
		if (!waterfall) {
			mesh = GetComponent<MeshFilter> ().mesh;
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < vertices.Length; i++) {
				vertices [i].z = 1 * Mathf.PingPong (Time.time * speed, randomHeights [i]);
			}
			mesh.vertices = vertices;
		}
	}
}
