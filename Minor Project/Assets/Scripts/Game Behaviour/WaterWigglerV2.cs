using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterWigglerV2 : MonoBehaviour {
	public Vector2 range = new Vector2(0.1f, 2f);
	public float speed = 1f;
	public bool waterfall = false;
	public float fallUpdateInS = 0.1f;
	public string waterfallMeshNumbers = "1"; //COMMA SEPARATED string of submesh numbers (count from 0) coresponding to a waterfall
	public GameObject ball;

	private float[] randomHeights;
	private float[] fallJiggle;
	private Mesh mesh;
	private float counter;
	private int[] riververts;
	private int[] fallverts;
	private Vector3[] originals;

	void Start(){
		
		mesh = GetComponent<MeshFilter>().mesh;	
		originals = mesh.vertices;

		//Debug.Log(mesh.subMeshCount);

		int[][] triangleList = new int[mesh.subMeshCount][];

		string[] indices = waterfallMeshNumbers.Split (',');
		int[] fallsubs = new int[indices.Length];
		int[] riversubs = new int[triangleList.Length - indices.Length];

		for (int i = 0; i < mesh.subMeshCount; i++) {
			triangleList [i] = mesh.GetTriangles (i);
		}

		List<int> excludeList = new List<int> ();
		foreach (int index in triangleList[1]) {
			foreach (int index2 in triangleList[0]) {
				if (mesh.vertices[index].Equals(mesh.vertices[index2])) {
					if (!excludeList.Contains (triangleList [1] [index])) {
						excludeList.Add (triangleList [1] [index]);
					}
					if (!excludeList.Contains (triangleList [0] [index2])) {
						excludeList.Add (triangleList [0] [index2]);
					}

				}
			}
			foreach (int index2 in triangleList[2]) {
				if (mesh.vertices[index].Equals(mesh.vertices[index2])) {
					Debug.Log("Duplicate: " + triangleList[1][index] + " " + triangleList[2][index2]);
					if (!excludeList.Contains (triangleList [1] [index])) {
						excludeList.Add (triangleList [1] [index]);
					}
					if (!excludeList.Contains (triangleList [2] [index2])) {
						excludeList.Add (triangleList [2] [index2]);
					}

				}
			}
		}

		string res = "";
		foreach (int value in excludeList) {
			res += value + " ";
		}
		Debug.Log(res);

		for (int i = 0; i < indices.Length; i++) {
			fallsubs[i]= int.Parse(indices[i]);
		}

		bool flag = false;
		int count = 0;

		for (int i = 0; i < triangleList.Length; i++) {

			for(int j = 0; j < fallsubs.Length; j++) {
				if (i == fallsubs [j]) {
					flag = true;
				}
			}
			if (!flag) {
				riversubs [count] = i;
				count++;
			}
			flag = false;
		}
			
		int countb = 0;
		for (int i = 0; i < riversubs.Length; i++) {
			countb += triangleList [riversubs [i]].Length;
		}
		int[] rivertriangles = new int[countb];

		int countc = 0;
		for (int i = 0; i < riversubs.Length; i++) {
			for (int j = 0; j < triangleList[riversubs[i]].Length; j++){
				rivertriangles [countc] = triangleList [riversubs [i]] [j];
				countc++;
			}
		}
		int countd = 0;
		for (int i = 0; i < fallsubs.Length; i++){
			countd += triangleList [fallsubs [i]].Length;
		}
		int[] falltriangles = new int[countd];

		int counte = 0;
		for (int i = 0; i < fallsubs.Length; i++) {
			for (int j = 0; j < triangleList [fallsubs [i]].Length; j++) {
				falltriangles [counte] = triangleList [fallsubs [i]] [j];
				counte++;
			}
		}

		List<int> riververtlist = new List<int>();

		for (int i = 0; i < rivertriangles.Length; i++) {
			Debug.Log (rivertriangles [i]);
			if(!riververtlist.Contains(rivertriangles[i]) && !excludeList.Contains(rivertriangles[i])){
				Debug.Log ("Added");
				riververtlist.Add (rivertriangles [i]);
			}
		}

		res = "";
		foreach (int value in riververtlist) {
			res += value + " ";
		}
		Debug.Log(riververtlist.Count + ": " + res);

		List<int> fallvertlist = new List<int> ();

		for (int i = 0; i < falltriangles.Length; i++) {
			if (!fallvertlist.Contains (falltriangles [i]) && !excludeList.Contains(falltriangles[i])) {
				fallvertlist.Add (falltriangles [i]);
			}
		}

		res = "";
		foreach (int value in fallvertlist) {
			res += value + " ";
		}
		Debug.Log(fallvertlist.Count + ": " + res);


		riververts = riververtlist.ToArray();
		fallverts = fallvertlist.ToArray();

		randomHeights = new float[riververts.Length];

		for (int i = 0; i < riververts.Length; i++) {
			randomHeights[i] = Random.Range(range.x, range.y);
		}

		fallJiggle = new float[fallverts.Length];

			

	}

	void Update() {
		//mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < riververts.Length; i++) {
			vertices [riververts[i]].z = originals [riververts[i]].z + Mathf.PingPong (Time.time * speed, randomHeights [i]);
		}
		counter += Time.deltaTime;

		if (counter > fallUpdateInS) {
			for (int i = 0; i < fallverts.Length; i++) {
				fallJiggle [i] = Random.Range (range.x, range.y);
			}
			for (int i = 0; i < fallverts.Length; i++) {
				vertices [fallverts[i]].x = originals[fallverts[i]].x + fallJiggle [i];
			}
			counter = 0f;
		}

		mesh.vertices = vertices;
	}
}
