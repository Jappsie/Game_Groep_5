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
	public float triggertime = 2f;
	public float repeatrate = 1f;

	// Use this for initialization
	void Start () {
	    
		//Begin positie + rotatie van turret in de wereld 
		startPos = gameObject.transform.position;
		startRot = gameObject.transform.rotation;

		// Zorgt ervoor dat de functie bullittrigger om de paar seconden wordt aangeroepen
		InvokeRepeating("BullitTrigger",triggertime,repeatrate);
	}
	

	void Update () {
		// zorgt ervoor dat de turret naar de player kijkt als hij in zijn line of sight is.

		Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;
		Quaternion objectRot = gameObject.transform.rotation;;


		if ( Vector3.Distance( playerPos, objectPos ) < LineofSight )
		{
			gameObject.transform.rotation = Quaternion.Slerp( objectRot, Quaternion.LookRotation( playerPos - objectPos ), rotationSpeed * Time.deltaTime );
		}
		//Zorgt ervoor dat de turret weer naar zijn beginpositie roteert op het moment dat de player uit zijn line of sight is
		else
		{
			gameObject.transform.rotation = Quaternion.Slerp( objectRot, startRot, rotationSpeed * Time.deltaTime );
		}
	}

	public void BullitTrigger(){
		// Als de player in zijn line of sight is zal er een kogel worden afgevuurd

		Player = GameObject.FindGameObjectsWithTag( "Player" )[ 0 ];
		Vector3 playerPos = Player.transform.position;
		Vector3 objectPos = gameObject.transform.position;

		if ( Vector3.Distance( playerPos, objectPos ) < LineofSight )
		{
			Instantiate (bullit, transform.position, transform.rotation);
		}
	}
}
