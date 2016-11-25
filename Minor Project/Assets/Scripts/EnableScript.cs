using UnityEngine;
using System.Collections;

public class EnableScript : MonoBehaviour {
	public GameObject[] toEnable;
	private bool curStatus;

	void Start() {
		curStatus = false;
		for (int i = 0; i < toEnable.Length; i++) {
			toEnable[i].SetActive (false);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			for (int i = 0; i < toEnable.Length; i++) {
				toEnable[i].gameObject.SetActive (!curStatus);
			}
			curStatus = !curStatus;
		}
	}
}
