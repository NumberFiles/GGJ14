using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
<<<<<<< HEAD
	protected CharacterMotor motor;
=======
	protected Controller control;
	protected CharacterMotor motor;
	
	public string[] weaponSlots = { "Pistol", "Rocket" };
	protected Dictionary<string, Weapon> slottedWeapons = new Dictionary<string, Weapon>();
>>>>>>> origin/master
	
	public float healthPoints {
		get;
		set;
	}
	
	public bool IsDead {
		get { return healthPoints <= 0.0f; }
	}
	
	
	public CharacterController characterController;
	
<<<<<<< HEAD
=======
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
	
>>>>>>> origin/master
	// Use this for initialization
	protected virtual void Start () {
		SetInitialProperties();
		characterController = GetComponent<CharacterController>();
<<<<<<< HEAD
=======
		control = GetComponent<Controller>();
>>>>>>> origin/master
		motor = GetComponent<CharacterMotor>();
	}
	
	void SetInitialProperties() {
		healthPoints = 100;
	}
}
