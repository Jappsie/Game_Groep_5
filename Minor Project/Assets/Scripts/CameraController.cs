using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;   // Object dat achtervolgt moet worden
    private Vector3 offset;

    // Vind het verschil tussen het object en de camera
	void Start () {
        offset = transform.position - player.transform.position;
	}
	
    // Verplaats mee met het object
	void LateUpdate () {
        float x = player.transform.position.x + offset.x;
		float z = player.transform.position.z + offset.z;
		gameObject.transform.position = new Vector3 (x, transform.position.y, z);
	}

}
