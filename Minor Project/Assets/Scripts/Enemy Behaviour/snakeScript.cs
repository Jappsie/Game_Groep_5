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

    public GameObject crystal;
    public float startTime; // Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
    public float intervalTime; // tussen maken van 2 stenen
    public float repeatTime; // Tijd tussen twee opeenvolgende sequences

	public List<Transform> BodyParts = new List<Transform> (); //Creates a new list with BodyParts of the snake 
	public float mindis = 0.25f;  //Minimum distance between two bodyparts
	public float speed = 10f; 
	public float rotationspeed = 50f; 


	public GameObject bodyPrefab;  //A prefab of the bodypart
	private Transform curBodyPart; 
	private Transform prevBodyPart; 
	private float distance; 


 
	void Start () {

		snakehead = GameObject.FindGameObjectWithTag ("SnakeHead"); 

		for (int i = 0; i < amount_bodyparts; i++) {
			AddBodyParts (); 
		}

        if (repeatTime < 5f* intervalTime)       //Als de repeatTime > 4*intervalTime, dan gaat het fout
        {
            repeatTime = 5f * intervalTime + 1;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        crystals = new Stack ();
		crystalSequence = false;
        Invoke("spawnRocks", startTime);
	}

	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
		snakehead = GameObject.FindGameObjectWithTag ("SnakeHead");
		if (!crystalSequence) {

			//Movement of the snakehead
			snakehead.transform.rotation = Quaternion.Slerp (snakehead.transform.rotation, Quaternion.LookRotation (player.transform.position - snakehead.transform.position), 4f * Time.deltaTime);
			snakehead.transform.position += snakehead.transform.forward * 10f * Time.deltaTime;

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

				float T = Time.deltaTime * distance / mindis * 10f; 
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
        for (int i = 0; i < 4; i++)
        {
            playerLocation = player.transform.position;
            playerLocation.y = -2.5f;
            GameObject curCrystal = Instantiate(crystal, playerLocation, Quaternion.identity) as GameObject;
            crystals.Push(curCrystal);
            yield return new WaitForSeconds(intervalTime);
            curCrystal.GetComponentInChildren<BoxCollider>().enabled = true;
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
					
					
}
