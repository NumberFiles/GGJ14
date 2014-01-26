using UnityEngine;
using System.Collections;

public class PlayerCamera : MouseLookCamera {
	CharacterMotor motor;
	
	public float wallRunTilt = 10.0f;
	protected float tilt = 0.0f;
	public float wallRunTiltRate = 20.0f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		float desiredTilt = motor.wallRunning * wallRunTilt;
		if(Mathf.Abs(desiredTilt - tilt) > wallRunTiltRate * Time.deltaTime)
			tilt += Mathf.Sign(desiredTilt - tilt) * wallRunTiltRate * Time.deltaTime;
		else
			tilt = desiredTilt;
		attachedCamera.transform.Rotate(0,0,tilt);
	}
}
