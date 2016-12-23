using UnityEngine;
using System.Collections;

public class BossFeedback : MonoBehaviour {

	private GameObject Boss;
	private Renderer[] cloudParts;

	// Use this for initialization
	void Start () {
		Boss = GameObject.FindWithTag ("Boss");
		cloudParts = this.GetComponentsInChildren<Renderer>();
		Debug.Log (cloudParts);
		for (int i = 0; i < cloudParts.Length; i++) {
			cloudParts [i].material.color = new Color (1f, 1f, 1f);
		}
	}
	
	// Update is called once per frame
	void Update () {
;
	}
}
