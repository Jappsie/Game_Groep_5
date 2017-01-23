using UnityEngine;
using System.Collections;

public class snakeScript : HealthSystem {

    private GameObject player;
    private Vector3 playerLocation;
    private Stack crystals;
	private bool crystalSequence;

	public float rotationSpeed = 4f;
	public float moveSpeed = 10f;
    public GameObject crystal;
	public int crystalAmount = 4;
    public float startTime; // Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
	public float damageDelay; //Tijd tussen het spawnen van de steen, en het activeren van de collider
    public float intervalTime; // tussen maken van 2 stenen
    public float repeatTime; // Tijd tussen twee opeenvolgende sequences
	public float spawnDepth = -2.5f; //Diepte waarop de steen spawnt

 
	void Start () {
		if (repeatTime < (crystalAmount + 1) * intervalTime)       //Als de repeatTime < 4*intervalTime, dan gaat het fout
        {
			repeatTime = (crystalAmount + 1) * intervalTime + 1;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        crystals = new Stack ();
		crystalSequence = false;
        Invoke("spawnRocks", startTime);
	}

	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
		if (!crystalSequence) {
			gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, Quaternion.LookRotation (player.transform.position - gameObject.transform.position), rotationSpeed * Time.deltaTime);
			gameObject.transform.position += gameObject.transform.forward * moveSpeed * Time.deltaTime;
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
}
