using UnityEngine;
using System.Collections;

public class EnableScript : HealthSystem {
	public GameObject[] toEnable;
    public string collideTag;
	private bool curStatus;
	private Vector3 spawnLocation;
	private Quaternion initialRotation;
	private Rigidbody RBody;

	void Start() {
		curStatus = false;
		for (int i = 0; i < toEnable.Length; i++) {
			toEnable[i].SetActive (false);
		}
		spawnLocation = this.gameObject.transform.position;
		initialRotation = this.gameObject.transform.rotation;
		RBody = this.gameObject.GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag(collideTag)) {
            Debug.Log("Activated");
			for (int i = 0; i < toEnable.Length; i++) {
				toEnable[i].gameObject.SetActive (!curStatus);
			}
			curStatus = !curStatus;
		}
	}

	protected override void Death() {
		this.gameObject.transform.position = spawnLocation;
		RBody.velocity = new Vector3 (0f, 0f, 0f);
		this.transform.rotation = initialRotation;
		RBody.freezeRotation = true;
		RBody.freezeRotation = false;
	}
}
