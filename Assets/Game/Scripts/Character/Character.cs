using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Light))]
public class Character : MonoBehaviour {
	
	public float healthPoints {
		get;
		set;
	}
	
	public bool IsDead {
		get { return healthPoints <= 0.0f; }
	}
	
	
	public CharacterController characterController;
	public Light EmittedLight;
	private Color currentLightColour;
	
	// Use this for initialization
	void Start () {
		SetInitialProperties();
		characterController = GetComponent<CharacterController>();
		EmittedLight = GetComponent<Light>();
	}
	
	// FixedUpdate is called once per game tick
	void FixedUpdate () {
		UpdateLightBasedOnVelocity();
	}
	
	void UpdateLightBasedOnVelocity() {
		float sqrMagnitude = characterController.velocity.sqrMagnitude;
		if (sqrMagnitude <= 0.0f) {
			currentLightColour = Color.red;
		}
		else if (sqrMagnitude > 5.0f) {
			currentLightColour = Color.blue;
		}
		EmittedLight.color = currentLightColour;
	}
	
	
	void SetInitialProperties() {
		healthPoints = 100;
	}
	void MakeRedder() {
		
	}
	
	void MakeBluer() {
	
	}
}
