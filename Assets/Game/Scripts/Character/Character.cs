using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
	protected Controller control;
	protected CharacterMotor motor;
	
	public string[] weaponSlots = { "Rocket" };
	protected Dictionary<string, Weapon> slottedWeapons = new Dictionary<string, Weapon>();
	
	public float healthPoints {
		get;
		set;
	}
	
	public bool IsDead {
		get { return healthPoints <= 0.0f; }
	}
	
	
	public CharacterController characterController;
	
	protected virtual void FixedUpdate() {
		Weapon[] weapons = GetComponentsInChildren<Weapon>();
		slottedWeapons.Clear();
		foreach(Weapon weapon in weapons) {
			slottedWeapons.Add(weapon.weaponSlot, weapon);
		}
		
		if(control.input.fire) {
			Weapon weapon = slottedWeapons[weaponSlots[control.input.weapon]];
			if(weapon != null) {
				control.input.fire = weapon.Fire(Quaternion.Euler(control.input.look) * Vector3.forward);
			} else {
				control.input.fire = false;
			}
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
		SetInitialProperties();
		characterController = GetComponent<CharacterController>();
		control = GetComponent<Controller>();
		motor = GetComponent<CharacterMotor>();
	}
	
	void SetInitialProperties() {
		healthPoints = 100;
	}
}
