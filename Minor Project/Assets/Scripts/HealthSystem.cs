//Author: Jasper van Bemmelen
//Version: 20/11/2016 22:08:00

using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour {

	public float MaxHealth;
	public float CurHealth;
	PlayerMovement playerMovement;

	private bool isDead;
	private bool damaged;

	//Initialiseer de relevante variabelen
	void Awake() {
		playerMovement = GetComponent<PlayerMovement> ();
		CurHealth = MaxHealth;	
	}

	//Functie die wordt aangeroepen als een Object schade krijgt
	public void TakeDamage(float damageAmount) {
		damaged = true;
		CurHealth -= damageAmount;

		//Als het Object geen hp meer heeft, dan gaat hij dood
		if (CurHealth <= 0 && !isDead) {
			Death ();
		}
	}

	void Death() {
		isDead = true;
		playerMovement.isDead = true;
	}
}
