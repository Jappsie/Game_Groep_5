using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class snakeScript : HealthSystem {

    private GameObject player;
    private Vector3 playerLocation;
    private Stack crystals;
    private bool crystalSequence;
    private GameObject snakehead; 
    public int amount_bodyparts; 
    public float rotationSpeed = 4f;
    public float moveSpeed = 10f;
    public GameObject crystal;
    public int crystalAmount = 4;
    public float startTime; // Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
    public float damageDelay; //Tijd tussen het spawnen van de steen, en het activeren van de collider
    public float intervalTime; // tussen maken van 2 stenen
    public float repeatTime; // Tijd tussen twee opeenvolgende sequences
	public float speedupTime; //Tijd waarop de intervalTime kleiner wordt

	public float spawnDepth = -2.5f; //Diepte waarop de steen spawnt

	public List<Transform> BodyParts = new List<Transform> (); //Creates a new list with BodyParts of the snake 
	public float minDistance = 0.25f;  //Minimum distance between two bodyparts
	public float speed = 10f; 
	public float rotationspeed = 50f; 


	public GameObject bodyPrefab;  //A prefab of the bodypart
	private Transform curBodyPart; 
	private Transform prevBodyPart; 
	private float distance; 


 
	void Start () {

		       //Als de repeatTime < 4*intervalTime, dan gaat het fout

		snakehead = GameObject.FindGameObjectWithTag ("SnakeHead"); 

		for (int i = 0; i < amount_bodyparts; i++) {
			AddBodyParts (); 
		}

		if (repeatTime < (crystalAmount + 1) * intervalTime){//Als de repeatTime > 4*intervalTime, dan gaat het fout

			repeatTime = (crystalAmount + 1) * intervalTime + 1;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        crystals = new Stack ();
		crystalSequence = false;
        Invoke("spawnRocks", startTime);
		InvokeRepeating ("speedUp", startTime, speedupTime);
	}

	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");

		snakehead = GameObject.FindGameObjectWithTag ("SnakeHead");
		if (!crystalSequence) {
			//Movement of the snakehead
			snakehead.transform.rotation = Quaternion.Slerp (snakehead.transform.rotation, Quaternion.LookRotation (player.transform.position - snakehead.transform.position), rotationSpeed * Time.deltaTime);
			snakehead.transform.position += snakehead.transform.forward * moveSpeed * Time.deltaTime;

			//Movement for every bodypart 
			for (int i = 0; i < BodyParts.Count; i++) {
				curBodyPart = BodyParts [i];
				//Wanneer het het eerste bodypart is, is het bodypart voor hem het snakehoofd.
				if(BodyParts[i].Equals(BodyParts[0])){
					prevBodyPart = snakehead.transform;
				}
				else {
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
				curBodyPart.position = Vector3.Slerp (curBodyPart.position,newpos,T);
				curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation,T);
			}

		}
	}


    void spawnRocks ()          //Method die de sequence start
    {
        Debug.Log(crystals.Count);
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
        yield return new WaitForSeconds(repeatTime);
        spawnRocks();
    }

	private void OnCollisionEnter(Collision col) {
		Debug.Log ("Collision met: " + col.gameObject.tag);
		if (col.gameObject.CompareTag ("Crystal")) {
			Debug.Log ("Damage werkt");
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
					


	private void speedUp() {
		Debug.Log ("intervaltime = " + intervalTime);
		intervalTime = Mathf.Max (intervalTime * 0.9f, 0.5f);
	}

}
