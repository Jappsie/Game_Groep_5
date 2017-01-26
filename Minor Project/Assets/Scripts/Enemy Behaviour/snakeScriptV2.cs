using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class snakeScriptV2 : HealthSystem
{

	private GameObject player;
	private Vector3 playerLocation;
	private Stack crystals;
	private bool crystalSequence;

	private bool Vulnerable;
	public float snekDamage = 1f;

	private bool Strike;
	private bool Close;
	private int Attack;
	//OM EEN OF ANDERE REDE HEEFT DE AUTO FORMAT ALLE COMMENTS ONDER DE BIJBEHORENDE VARIABLE GEZET
	private Vector3 Telegraph;
	// target positie voor het snekhoofd voor een strike
	private Vector3 strikePos;
	// target positie voor het snekhoofd om naar te 'striken'. momentopname van de playerpositie
	private Vector3 prevPosistion;
	// positie van het snekhoofd voordat de strike sequence begon, om hier na de aanval naar terug te keren

	private GameObject snakehead;
	private Transform curBodyPart;
	private Transform prevBodyPart;
	private float distance;
	private CharacterController controller;

	public int amount_bodyparts;

	public float rotationSpeed = 4f;
	// rotation speed of the snek
	public float moveSpeed = 10f;
	// Movement speed of the snek
	public float playerDistance = 2f;
	//maximum distance the player should have from the snek before the 'strike' attack takes place
	public GameObject crystal;
	public int crystalAmount = 4;
	// amount of crystals spawned during attack cycle
	public float startTime;
	// Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
	public float damageDelay;
	//Tijd tussen het spawnen van de steen, en het activeren van de collider
	public float intervalTime;
	// tussen maken van 2 stenen
	public float spawnDepth = -2.5f;
	//Diepte waarop de steen spawnt

	public List<Transform> BodyParts = new List<Transform> ();
	//Creates a new list with BodyParts of the snake
	public float minDistance = 0.25f;
	//Minimum distance between two bodyparts

	public float animHeight = 2f;
	// Hoogte die het hoofd van de slang stijgt voor de strike aanval
	public float riseSpeed = 5f;
	// snelheid waarmee het hoof stijgt voor de strike aanval
	public float strikeSpeed = 20f;
	// snelheid waarmee de snek striked naar de player
	public float retreatSpeed = 3f;
	// snelheid na de strike, waarmee het hoofd terug in de originele positie gaat
	public float coolDown = 3f;
	// interval tussen een strike en het starten van een crystalSequence

	public GameObject bodyPrefab;
	//A prefab of the bodypart




	void Start ()
	{
		Vulnerable = false;
		Attack = 0;
		snakehead = GameObject.FindGameObjectWithTag ("SnakeHead"); 

		for (int i = 0; i < amount_bodyparts; i++) {
			AddBodyParts (); 
		}
			
		player = GameObject.FindGameObjectWithTag ("Player");
		crystals = new Stack ();
		crystalSequence = false;
		Strike = false;
		Close = false;
		Invoke ("spawnRocks", startTime);
		controller = player.GetComponent<CharacterController> ();
	}
		
	void Update ()
	{
		//check player distance to snek
		if (Vector3.Distance (snakehead.transform.position, player.transform.position) <= playerDistance) {
			Close = true;
		} else {
			Close = false;
		}
		//check if the snek is attacking, and move snek if it isn't
		if (!crystalSequence && !Strike && !Close) {
			snekMovement ();
		}

		//check if player is standing still in a close enough vinvcinity to the snek for it to strike
		//then set up the coordinates for striking
		if (Close && !Strike && !crystalSequence && controller.velocity.x == 0 && controller.velocity.z == 0) {

			Strike = true;
			Telegraph = snakehead.transform.position + new Vector3 (0f, animHeight, 0f);
			strikePos = player.transform.position;
			prevPosistion = snakehead.transform.position;

		}
		//Execute snekAttack
		if (Strike && !crystalSequence) {
			snekAttack ();
		}
	}

	void snekMovement ()
	{
		//Rotation of the snakehead
		snakehead.transform.rotation = Quaternion.Slerp (snakehead.transform.rotation, Quaternion.LookRotation (player.transform.position - snakehead.transform.position), rotationSpeed * Time.deltaTime);
		//Move snakehead
		snakehead.transform.position += snakehead.transform.forward * moveSpeed * Time.deltaTime;
		//Movement for every bodypart 
		for (int i = 0; i < BodyParts.Count; i++) {
			curBodyPart = BodyParts [i];
			//Wanneer het het eerste bodypart is, is het bodypart voor hem het snakehoofd.
			if (BodyParts [i].Equals (BodyParts [0])) {
				prevBodyPart = snakehead.transform;
			} else {
				prevBodyPart = BodyParts [i - 1];
			}

			//De afstand tussen het huidige bodypart en zijn voorganger
			distance = Vector3.Distance (prevBodyPart.position, curBodyPart.position);
			Vector3 newpos = prevBodyPart.position; 
			newpos.y = BodyParts [0].position.y;

			//De snelheid waarmee bodyparts bewegen, afhankelijk van de afstand tot hun voorganger, capped op 0.5f
			float T = Time.deltaTime * distance / minDistance * 10f; 
			if (T > 0.5f)
				T = 0.5f;

			//Verplaatst het currentbodypart naar de newpos bodypart.
			curBodyPart.position = Vector3.Slerp (curBodyPart.position, newpos, T);
			curBodyPart.rotation = Quaternion.Slerp (curBodyPart.rotation, prevBodyPart.rotation, T);
		}
	}

	void snekAttack ()
	{
		//beweeg het hoofd omhoog om de de aanval aan de speler te communiceren
		if (Attack == 0) {
			snakehead.transform.position = Vector3.MoveTowards (snakehead.transform.position, Telegraph, riseSpeed * Time.deltaTime);
			//als het hoofd in positie is, vervolg de volgende stap van de aanval en maar het hoofd vulnerable voor eenmaal damage van een crystal
			if (Vector3.Distance (snakehead.transform.position, Telegraph) <= 0.1) {
				Vulnerable = true;
				Attack = 1;
			}
		}
		//strike het snekhoofd naar de opgeslagen positie van de speler
		if (Attack == 1) {
			snakehead.transform.position = Vector3.MoveTowards (snakehead.transform.position, strikePos, strikeSpeed * Time.deltaTime);
			//als de strike voltooid is, activeer de volgende stap
			if (Vector3.Distance (snakehead.transform.position, strikePos) <= 0.1) {
				Attack = 2;
			}
		}
		//trek het hoofd van de slang terug naar de positie van voor de aanval begon
		if (Attack == 2) {
			snakehead.transform.position = Vector3.MoveTowards (snakehead.transform.position, prevPosistion, retreatSpeed * Time.deltaTime);
			//Als de slang weer in zijn originele staat terug is, markeer aanval als voltooid en start de volgende crystal sequence na coolDown seconden
			if (Vector3.Distance (snakehead.transform.position, prevPosistion) <= 0.1) {
				Strike = false;
				Attack = 0;
				crystalSequence = true;
				Invoke ("spawnRocks", coolDown);
			}
		}
	}

	void spawnRocks ()          //Method die de sequence start
	{

		foreach (GameObject curCryst in crystals) {       //Verwijder alle bestaande stenen
			Destroy (curCryst);
		}
		crystals.Clear ();
		crystalSequence = true;
		StartCoroutine ("spawnRock");        //start het spawnen van de stenen
	}

	IEnumerator spawnRock ()    //Method die het spawnen van de stenen regelt
	{
		for (int i = 0; i < crystalAmount; i++) {
			playerLocation = player.transform.position;
			playerLocation.y = spawnDepth;
			GameObject curCrystal = Instantiate (crystal, playerLocation, Quaternion.identity) as GameObject;
			curCrystal.GetComponentInChildren<BoxCollider> ().enabled = false;
			crystals.Push (curCrystal);
			yield return new WaitForSeconds (damageDelay);
			curCrystal.GetComponentInChildren<BoxCollider> ().enabled = true;
			yield return new WaitForSeconds (intervalTime);
		}
		crystalSequence = false;
	}
	//loopt 1 keer als de slang vulnerable is, zodat 1 crystal de slang 1 keer kan damagen per aanval.
	private void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("Crystal") && Vulnerable) {
			TakeDamage (snekDamage);
			Vulnerable = false;
		}
	}

	protected override void Death ()
	{
		Destroy (gameObject);
	}

	//Add Bodyparts to the list of BodyParts
	public void AddBodyParts ()
	{
		Transform newPart = (Instantiate (bodyPrefab, BodyParts [BodyParts.Count].position, BodyParts [BodyParts.Count].rotation) as GameObject).transform; 
		newPart.SetParent (transform); 
		BodyParts.Add (newPart); 

	}


}
