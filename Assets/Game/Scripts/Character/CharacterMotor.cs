using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {
	protected CharacterController character;
	
	public class Control {
		public Vector2 move = Vector2.zero;
		public Vector3 lookDirection = Vector2.zero;
		public bool jump;
	}
	public Control control = new Control();
	
	public class Movement {
		public float runSpeed = 15.0f; //Running speed in units per second
		public float sqrunSpeed { get { return runSpeed * runSpeed; } }
		public float groundFriction = 20.0f; //Deceleration of grounded player going above run speed in units per second per second
		public float airFriction = 1.0f; //Deceleration of player in the air in units per second per second
	}
	public Movement move = new Movement();
	
	public class Sliding {
		public float slidingSpeed = 20.2f; //Running speed in units per second
		public float sqrSlidingSpeed { get { return slidingSpeed * slidingSpeed; } }
		public float stopRate = 5.0f; //Rate of deceleration when player opposes movement above sliding speed in units per second per second
	}
	public Sliding slide = new Sliding();
	
	public class Jumping {
		public float vJumpImpulse = 1.0f; //Players vertical change in speed when jumping up in units per second
		public float dJumpImpulse = 1.0f; //Players horizontal change in speed when jumping in a direction in units per second
		public float dJumpVertImpulse = 0.8f; //Players vertical change in speed when jumping in a direction in units per second
		public float earlyJumpPenalty = 0.2f; //Players horizontal loss of speed for jumping early as a fraction
	}
	public Jumping jump = new Jumping();
	
	// Use this for initialization
	void Start () {
		character = GetComponent<CharacterController>();
		control.lookDirection = transform.forward;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UnityEngine.Debug.Log("Fixed update in character motor");
		//get our horizontal velocity
		Vector3 velocity = character.velocity;
		velocity.y = 0;
		
		if(character.isGrounded) {
			//calculate our movement input direction in world space
			Vector3 worldMove = new Vector3(control.move.x, 0, control.move.y);
			worldMove = transform.rotation * worldMove;
			
			if(velocity.sqrMagnitude <= slide.sqrSlidingSpeed) {
				velocity = move.runSpeed * worldMove;
			} else {
				Vector3 velDir = velocity.normalized;
				Vector3 accel = Vector3.zero;
				accel -= velDir * move.groundFriction;
				accel += Mathf.Clamp(Vector3.Dot(worldMove, velDir), -1f, 0f) * velDir;
				velocity += accel * Time.deltaTime;
			}
		} else {
		}
		if(control.jump) {
			velocity *= 2;
			control.jump = false;
		}
		character.SimpleMove(velocity);
	}
}
