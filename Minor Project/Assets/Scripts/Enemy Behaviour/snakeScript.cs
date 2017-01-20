using UnityEngine;
using System.Collections;

public class snakeScript : MonoBehaviour {

    private GameObject player;
    private Vector3 playerLocation;
    private Stack crystals;

    public GameObject crystal;
    public float startTime; // Tijd tussen het starten van de scene, en het beginnen van de eerste spawn sequence
    public float intervalTime; // tussen maken van 2 stenen
    public float repeatTime; // Tijd tussen twee opeenvolgende sequences
 
	void Start () {
        if (repeatTime < 5* intervalTime)       //Als de repeatTime > 4*intervalTime, dan gaat het fout
        {
            repeatTime = 5f * intervalTime + 1;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating( "spawnRocks", startTime, repeatTime);
        crystals = new Stack ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void spawnRocks ()          //Method die de sequence start
    {
        Debug.Log(crystals.Count);
        foreach (GameObject curCryst in crystals)       //Verwijder alle bestaande stenen
        {
           Destroy(curCryst);
        }
        StartCoroutine("spawnRock");        //start het spawnen van de stenen
    }

    IEnumerator spawnRock ()    //Method die het spawnen van de stenen regelt
    {
        for (int i = 0; i < 4; i++)
        {
            playerLocation = player.transform.position;
            playerLocation.y = -4f;
            Debug.Log(playerLocation);
            GameObject curCrystal = (GameObject) Instantiate(crystal, playerLocation, Quaternion.identity);
            crystals.Push(curCrystal);
            yield return new WaitForSeconds(intervalTime);
            curCrystal.transform.position += Vector3.up * 4f;
        }
    }
}
