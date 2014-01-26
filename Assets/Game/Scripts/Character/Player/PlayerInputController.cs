using UnityEngine;
using System.Collections;

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
		
		if(Input.GetKeyDown(KeyCode.Alpha1))
			input.weapon = 0;
		if(Input.GetKeyDown(KeyCode.Alpha2))
			input.weapon = 1;
		if(Input.GetKeyDown(KeyCode.Alpha3))
			input.weapon = 2;
		if(Input.GetKeyDown(KeyCode.Alpha4))
			input.weapon = 3;
		if(Input.GetKeyDown(KeyCode.Alpha5))
			input.weapon = 4;
		if(Input.GetKeyDown(KeyCode.Alpha6))
			input.weapon = 5;
		if(Input.GetKeyDown(KeyCode.Alpha7))
			input.weapon = 6;
		if(Input.GetKeyDown(KeyCode.Alpha8))
			input.weapon = 7;
		if(Input.GetKeyDown(KeyCode.Alpha9))
			input.weapon = 8;
		if(Input.GetKeyDown(KeyCode.Alpha0))
			input.weapon = 9;
		
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
