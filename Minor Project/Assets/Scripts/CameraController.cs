using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;   // Followed object
    private Vector3 offset;     // Relative position to followed object

    // Get the relative position to followed object
    void Start()
    {
        offset = gameObject.transform.position - player.transform.position;
    }

    // Follow the object
    void LateUpdate()
    {
        float x = player.transform.position.x + offset.x;
        float z = player.transform.position.z + offset.z;
        gameObject.transform.position = new Vector3( x, transform.position.y, z );
    }

}
