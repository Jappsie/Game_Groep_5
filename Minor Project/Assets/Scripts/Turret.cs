using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	private GameObject Player;
	public float LineofSight = 10f;
	public float rotationSpeed = 3.0f;
	private Vector3 startPos;
	public GameObject bullit;
	public GameObject bullitspawn;
	private Quaternion startRot;

	// Use this for initialization
	void Start () {
	 
		startPos = gameObject.transform.position;
		startRot = gameObject.transform.rotation;


		InvokeRepeating("BullitTrigger",2f,1f);
	}
	
	// Update is called once per frame
	void Update () {
		Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;;


		if ( Vector3.Distance( playerPos, objectPos ) < LineofSight )
		{
			gameObject.transform.rotation = Quaternion.Slerp( objectRot, Quaternion.LookRotation( playerPos - objectPos ), rotationSpeed * Time.deltaTime );
		}
		else
		{
			gameObject.transform.rotation = Quaternion.Slerp( objectRot, startRot, rotationSpeed * Time.deltaTime );
		}
	}

	public void BullitTrigger(){
		Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;

		if ( Vector3.Distance( playerPos, objectPos ) < LineofSight )
		{
			Instantiate (bullit, transform.position, transform.rotation);
		}
	}
}
