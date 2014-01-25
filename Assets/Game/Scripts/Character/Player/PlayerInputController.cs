using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
public class PlayerInputController : Controller {
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump"))
			input.jump = true;
		if(Input.GetButtonUp("Jump"))
			input.jump = false;
		
		if(Input.GetButtonDown("Wall Run"))
			input.wallRun = true;
		if(Input.GetButtonUp("Wall Run"))
			input.wallRun = false;
		
		if(Input.GetButtonDown("Fire1"))
			input.fire = true;
		if(Input.GetButtonUp("Fire1"))
			input.fire = false;
		
		input.move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		
		input.look.y += Input.GetAxis("Mouse X");
		if(input.look.y > 360)
			input.look.y -= 360;
		else if(input.look.y < 0)
			input.look.y += 360;
		
		input.look.x -= Input.GetAxis("Mouse Y");
		input.look.x = Mathf.Clamp(input.look.x, -90f, 90f);
	}
}
