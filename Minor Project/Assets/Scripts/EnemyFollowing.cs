using UnityEngine;
using System.Collections;

public class EnemyFollowing : MonoBehaviour
{
    public float followDistance = 10f;
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 3.0f;

    private GameObject Player;
    private Vector3 startPos;

    // Use this for initialization
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Follow a 'player' if within followDistance
    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
        Vector3 playerPos = Player.transform.position;
        Vector3 objectPos = gameObject.transform.position;
        Quaternion objectRot = gameObject.transform.rotation;

        if ( Vector3.Distance( playerPos, objectPos ) < followDistance )
        {
            gameObject.transform.rotation = Quaternion.Slerp( objectRot, Quaternion.LookRotation( playerPos - objectPos ), rotationSpeed * Time.deltaTime );
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Slerp( objectRot, Quaternion.LookRotation( startPos - objectPos ), rotationSpeed * Time.deltaTime );
        }
        gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
    }

}