using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class DeathCounter : MonoBehaviour {

	public Text deathText;

	// Use this for initialization
	void Start () {
	

		deathText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		deathText.text = PlayerPrefs.GetInt ("Deaths").ToString();
	
	}
}
