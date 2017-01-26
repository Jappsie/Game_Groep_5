using UnityEngine;
using System.Collections;

public class CrystalScript : MonoBehaviour {

	public GameObject parentSnake;

    private SnakeBoss SnakeBoss;

    private void Start()
    {
        SnakeBoss = parentSnake.GetComponent<SnakeBoss>();
    }

    private void OnCollisionEnter( Collision collision )
    {
        Debug.Log( "Enter" );
    }

    private void OnCollisionStay( Collision collision )
    {
        Debug.Log( "Stay" );
    }

    private void OnTriggerEnter( Collider other )
    {
        Debug.Log( "TEnter" );
    }

    private void OnTriggerStay( Collider other )
    {
        Debug.Log( "TStay" );
    }


}