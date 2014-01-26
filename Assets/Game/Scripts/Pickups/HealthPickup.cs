using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup {
	public float healthRecovered = 10f;
	public float overHealRatio = 1.0f;
	public float maxOverHeal = 1.5f;
	
	// Update is called once per frame
	public override void Grab(Character character) {
		if(playerOnly && character.tag != "Player")
			return;
		else if(character.healthPoints >= maxOverHeal * character.baseHealth)
			return;
		else if(character.healthPoints + healthRecovered > character.baseHealth) {
			float overHeal = healthRecovered - Mathf.Max(0.0f, character.baseHealth - character.healthPoints);
			overHeal *= overHealRatio;
			if(character.healthPoints < character.baseHealth)
				character.healthPoints = character.baseHealth;
			if(character.healthPoints + overHeal > maxOverHeal * character.baseHealth)
				character.healthPoints = maxOverHeal * character.baseHealth;
			else
				character.healthPoints += overHeal;
			Destroy (gameObject);
				
		} else {
			character.healthPoints += healthRecovered;
			Destroy (gameObject);
		}
	}
}
