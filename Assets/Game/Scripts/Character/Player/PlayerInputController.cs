using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
public class PlayerInputController : MonoBehaviour {
	protected CharacterMotor motor;
	
	// Use this for initialization
	void Start () {
		motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump"))
			motor.control.jump = true;
		motor.control.move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
	}
}
