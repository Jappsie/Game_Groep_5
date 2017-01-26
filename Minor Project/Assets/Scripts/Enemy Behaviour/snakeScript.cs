using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class snakeScript : HealthSystem {

    private GameObject player;
    private Vector3 playerLocation;
    private Stack crystals;
	private bool crystalSequence;

	private bool Strike;
	private bool Close;
	private int Attack;

	private Vector3 Telegraph; // target positie voor het snekhoofd voor een strike
	private Vector3 strikePos; // target positie voor het snekhoofd om naar te 'striken'. momentopname van de playerpositie
	private Vector3 prevPosistion; // positie van het snekhoofd voordat de strike sequence begon, om hier na de aanval naar terug te keren

	private GameObject snakehead; 
	private Transform curBodyPart; 
	private Transform prevBodyPart; 
	private float distance; 
	private CharacterController controller;

	public int amount_bodyparts; 

	public float rotationSpeed = 4f; // rotation speed of the snek
	public float moveSpeed = 10f; // Movement speed of the snek
	public float playerDistance = 2f; //maximum distance the player should have from the snek before the 'strike' attack takes place
    public GameObject crystal;
	public int crystalAmount = 4; // amount of crystals spawned during attack cycle
    public float startTime; // Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
	public float damageDelay; //Tijd tussen het spawnen van de steen, en het activeren van de collider
    public float intervalTime; // tussen maken van 2 stenen
    public float repeatTime; // Tijd tussen twee opeenvolgende sequences
	public float spawnDepth = -2.5f; //Diepte waarop de steen spawnt

	public List<Transform> BodyParts = new List<Transform> (); //Creates a new list with BodyParts of the snake 
	public float minDistance = 0.25f;  //Minimum distance between two bodyparts

	public float animHeight = 2f; // Hoogte die het hoofd van de slang stijgt voor de strike aanval
	public float riseSpeed = 5f; // snelheid waarmee het hoof stijgt voor de strike aanval
	public float strikeSpeed = 20f; // snelheid waarmee de snek striked naar de player
	public float retreatSpeed = 3f; // snelheid na de strike, waarmee het hoofd terug in de originele positie gaat
	public float coolDown = 3f; // interval tussen een strike en het starten van een crystalSequence

	public GameObject bodyPrefab;  //A prefab of the bodypart



 
	void Start () {
		
		Attack = 0;
		snakehead = GameObject.FindGameObjectWithTag ("SnakeHead"); 

		for (int i = 0; i < amount_bodyparts; i++) {
			AddBodyParts (); 
		}

		if (repeatTime < (crystalAmount + 1) * intervalTime){	//Als de repeatTime > 4*intervalTime, dan gaat het fout

			repeatTime = (crystalAmount + 1) * intervalTime + 1;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        crystals = new Stack ();
		crystalSequence = false;
		Strike = false;
		Close = false;
        Invoke("spawnRocks", startTime);
		controller = player.GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update () {
		
		if (!crystalSequence && !Strike) {
			//Movement of the snakehead
			snakehead.transform.rotation = Quaternion.Slerp (snakehead.transform.rotation, Quaternion.LookRotation (player.transform.position - snakehead.transform.position), rotationSpeed * Time.deltaTime);

			//check player distance to snek
			if (Vector3.Distance (snakehead.transform.position, player.transform.position) <= playerDistance) {
				Close = true;
			} else {
				Close = false;
			}
			//if player is not too close, move snek
			if(!Close){
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

					float T = Time.deltaTime * distance / minDistance * 10f; 
					if (T > 0.5f)
						T = 0.5f;

					//Verplaatst het currentbodypart naar de newpos bodypart.
					curBodyPart.position = Vector3.Slerp (curBodyPart.position, newpos, T);
					curBodyPart.rotation = Quaternion.Slerp (curBodyPart.rotation, prevBodyPart.rotation, T);
				}
			}
		}
		if(Close && !Strike && !crystalSequence && controller.velocity.x == 0 && controller.velocity.z == 0){
			Strike = true;
			Telegraph = snakehead.transform.position + new Vector3 (0f, animHeight, 0f);

			strikePos = player.transform.position;

			prevPosistion = snakehead.transform.position;

		}

		if (Strike && !crystalSequence) {
			if (Attack == 0) {
				Attack = 1;
				Debug.Log ("switch 1");
			} else if (Attack == 1 && Vector3.Distance(snakehead.transform.position, Telegraph) <= 0.1)  {
				Debug.Log ("switch 2");
				Attack = 2;
			} else if (Attack == 2 && Vector3.Distance(snakehead.transform.position, strikePos) <= 0.1) {
				Debug.Log ("switch 3");
				Attack = 3;
			} else if (Attack == 3 && Vector3.Distance(snakehead.transform.position, prevPosistion) <= 0.1){
				Debug.Log ("switch 0");
				Strike = false;
				// TODO clear crystals
				Attack = 0;
				crystalSequence = true;
				Invoke("spawnRocks", coolDown);

			}
		}

		switch(Attack){
		case 0: break;
		case 1: 
			snakehead.transform.position = Vector3.MoveTowards (snakehead.transform.position, Telegraph, riseSpeed * Time.deltaTime);
			break;
		case 2:
			//TODO attackinterval
			snakehead.transform.position = Vector3.MoveTowards(snakehead.transform.position, strikePos, strikeSpeed *Time.deltaTime);
			break;
		case 3:
			snakehead.transform.position = Vector3.MoveTowards(snakehead.transform.position, prevPosistion, retreatSpeed *Time.deltaTime);
			break;
		}
	}


    void spawnRocks ()          //Method die de sequence start
    {
       
        foreach (GameObject curCryst in crystals)       //Verwijder alle bestaande stenen
        {
           Destroy(curCryst);
        }
        crystals.Clear();
		crystalSequence = true;
        StartCoroutine("spawnRock");        //start het spawnen van de stenen
    }

    IEnumerator spawnRock ()    //Method die het spawnen van de stenen regelt
    {
        for (int i = 0; i < crystalAmount; i++)
        {
            playerLocation = player.transform.position;
			playerLocation.y = spawnDepth;
            GameObject curCrystal = Instantiate(crystal, playerLocation, Quaternion.identity) as GameObject;
			curCrystal.GetComponentInChildren<BoxCollider>().enabled = false;
            crystals.Push(curCrystal);
            yield return new WaitForSeconds(damageDelay);
            curCrystal.GetComponentInChildren<BoxCollider>().enabled = true;
			yield return new WaitForSeconds(intervalTime);
        }
		crystalSequence = false;
//        yield return new WaitForSeconds(repeatTime);
//        spawnRocks();
    }

	private void OnCollisionEnter(Collision col) {
		if (col.gameObject.CompareTag ("Crystal")) {
			TakeDamage (1f);
		}
	}

	protected override void Death ()
	{
		Destroy (gameObject);
	}

	//Add Bodyparts to the list of BodyParts
	public void AddBodyParts(){
		Transform newPart = (Instantiate (bodyPrefab, BodyParts [BodyParts.Count].position, BodyParts [BodyParts.Count].rotation) as GameObject).transform; 
		newPart.SetParent (transform); 
		BodyParts.Add(newPart); 
					
	}
					
					
}
