using UnityEngine;
using System.Collections;

public class QueenBee : HealthSystem {
	public float speed = 1.0f;
	public int PingPongRange = 6;
	public GameObject Eggs;              
	public bool eggCapability;                  //determines if enemy is capable of laying eggs
	public int maxEggs = 5;                     //Max amount of eggs a enemy can lay
	public int Birthtime = 10;                  // ammount of seconds before enemy starts laying eggs
	public int MinBirthRepeatTime = 1;          // Minumum Birth repeatrate  
	public int MaxBirthRepeatTime = 10;         //Maximum Birth repeatrate

	private Vector3 pos1;
	private Vector3 pos2;
	public GameObject[] clouds;
	private Renderer[][] cloudRenderer;


	// Use this for initialization
	void Start () {
		//queenbee moves between pos1 and pos 2
		pos1 = new Vector3 (transform.position.x - PingPongRange, transform.position.y, transform.position.z);
		pos2 = new Vector3 (transform.position.x + PingPongRange, transform.position.y, transform.position.z);
		clouds = GameObject.FindGameObjectsWithTag ("Cloud");
		cloudRenderer = new Renderer[clouds.Length][];
		for (int i = 0; i < clouds.Length; i++) {
			Debug.Log (i);
			cloudRenderer [i] = clouds [i].gameObject.GetComponentsInChildren<Renderer> ();
			//Debug.Log (cloudRenderer [i]);
		}

		int start_Birth = UnityEngine.Random.Range (1, Birthtime);
		int Birth_repeat = UnityEngine.Random.Range (MinBirthRepeatTime, MaxBirthRepeatTime);
		InvokeRepeating("Lay_Eggs", start_Birth, Birth_repeat);
	}

	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
		Physics.IgnoreCollision (gameObject.GetComponent<Collider>(), Eggs.GetComponent<CapsuleCollider>());
	}


	void Lay_Eggs(){
	//  instantiates a random number of eggs
		if (eggCapability) {
			int number = UnityEngine.Random.Range (1, maxEggs);
			for (int i = 0; i < number; i++) {
				Instantiate (Eggs, gameObject.transform.position + new Vector3(0f, 1f, 0f), Quaternion.FromToRotation(Vector3.up, Vector3.forward));
			}
		}
	}

	protected override void Death() {
		Destroy (gameObject);
	}

	private void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("PlayerBullet")) {
			Debug.Log ("test");
			Destroy (other.gameObject);
			TakeDamage (1);
			for (int i = 0; i < clouds.Length; i++) {
				for (int j = 0; j < cloudRenderer [i].Length; j++) {
					cloudRenderer [i] [j].material.color = new Color (1f, CurHealth/MaxHealth, CurHealth/MaxHealth);
				}
			}
		}
	}
	
}