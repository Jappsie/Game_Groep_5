using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class CharacterSelection : MonoBehaviour {


	private GameObject[] gadgetList;
	private int index; 

	// Use this for initialization
	void Start () {

		index = PlayerPrefs.GetInt ("CharacterSelected");

		//Fill the array with models 
		gadgetList = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			gadgetList [i] = transform.GetChild (i).gameObject;
		}
			//Show no models
			foreach (GameObject go in gadgetList) {
				go.SetActive (false); 
			}

		gadgetList[index].SetActive (true);
	
		}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.LeftArrow)) {
			Change1 (); 
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			Change2 (); 
			
	
	}

			
}
	public void Change1(){
		gadgetList [index].SetActive (false);
		index--; 
		if (index < 0){
			index = gadgetList.Length - 1;
		}
		gadgetList[index].SetActive(true);
}
	public void Change2(){
		gadgetList [index].SetActive (false);
		index++; 
		if (index == gadgetList.Length){
			index = 0;
		}
		gadgetList[index].SetActive(true);
	}

	public void ConfirmClicked(){
		PlayerPrefs.SetInt ("CharacterSelected", index);
		SceneManager.LoadScene ("main menu");

		
	
	}


}



		

