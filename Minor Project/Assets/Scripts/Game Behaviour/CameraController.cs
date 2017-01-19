using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;   // Followed object
    public bool trackObject;
    public bool useSlerp;
    public bool stickToGround;
    public float speed = 3f;
    private Vector3 offset;      // Relative position to followed object
    private float CameraPos;


    // Get the relative position to followed object
    void Start()
    {
        CameraPos = gameObject.transform.position.y;

        if ( trackObject )
        {
            gameObject.transform.LookAt( player.transform.position, Vector3.up );
        }
        offset = gameObject.transform.position - player.transform.position;
    }

    // Follow the object
    void LateUpdate()
    {
        float x = player.transform.position.x + offset.x;
        float z = player.transform.position.z + offset.z;
        Vector3 targetPos1 = new Vector3( x, player.transform.position.y + CameraPos, z );
        Vector3 targetPos2 = new Vector3( x, transform.position.y, z );


        if ( useSlerp )
        {
            if ( !stickToGround || player.GetComponent<CharacterController>().isGrounded )
            {
                gameObject.transform.position = Vector3.Slerp( targetPos2, targetPos1, speed * Time.deltaTime );
            }
            else
            {
                gameObject.transform.position = targetPos2;
            }
        }
        else
        {
            gameObject.transform.position = targetPos1;
        }
    }

}
