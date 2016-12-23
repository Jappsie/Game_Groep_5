using UnityEngine;
using System.Collections;

public class EnableScript : MonoBehaviour {
	public GameObject[] toEnable;
    public string collideTag;
	public bool repeat;

	private bool curStatus;

	void Start() {
		curStatus = false;
		for (int i = 0; i < toEnable.Length; i++) {
			toEnable[i].SetActive (false);
		}
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag(collideTag)) {
            Debug.Log("Activated");
			for (int i = 0; i < toEnable.Length; i++) {
				if (repeat) {
					toEnable [i].gameObject.SetActive (!curStatus);
				} else {
					toEnable [i].gameObject.SetActive (true);
				}
			}
			curStatus = !curStatus;
		}
	}
}