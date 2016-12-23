using UnityEngine;
using System.Collections;

public class MeshDebug : MonoBehaviour {
	private Mesh mesh;
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		Debug.Log (mesh.subMeshCount);
		int[][] triangleList = new int[mesh.subMeshCount][];
		for (int i = 0; i < mesh.subMeshCount; i++) {
			triangleList [i] = mesh.GetTriangles (i);
			Debug.Log (triangleList [i].Length);
		}
		for (int i = 0; i < mesh.subMeshCount-1; i++) {
			foreach (int index in triangleList[1]) {
				foreach (int index2 in triangleList[i+1]) {
					if (mesh.vertices[index].Equals(mesh.vertices[index2])) {
						Debug.Log("Duplicate: " + mesh.vertices[index]);
					}
				}
			}
		}

	}


}
