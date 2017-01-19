using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ControlCanvas : MonoBehaviour {


	public Image Controls; 
	public Canvas GETIT;


	void Start()
	{
		PlayerMovement.AbleShoot = false; 
	}


	public void GetItClick ()
	{
		Controls.CrossFadeAlpha (0.0f, 0.1f, false);
		GETIT.gameObject.SetActive (false); 
		PlayerMovement.AbleShoot = true; 
	


	}
}
