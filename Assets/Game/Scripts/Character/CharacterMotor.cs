using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {
	protected CharacterController character;
	protected bool jumpedEarly = false;
	
	public class Control {
		public Vector2 move = Vector2.zero;
		public Vector2 look = Vector2.zero;
		public bool jump;
	}
	public Control control = new Control();
	
	[System.Serializable]
	public class Movement {
		public float runSpeed = 12.0f; //Running speed in units per second
		public float sqrunSpeed { get { return runSpeed * runSpeed; } }
		public float groundFriction = 40.0f; //Deceleration of grounded player going above run speed in units per second per second
		public float airFriction = 1 / 15.0f; //Players air friction. Multiply by velocity to get deceleration
		public float gravity = 9.8f;
	}
	public Movement move = new Movement();
	
	[System.Serializable]
	public class Sliding {
		public float slidingSpeed = 12.2f; //Running speed in units per second
		public float sqrSlidingSpeed { get { return slidingSpeed * slidingSpeed; } }
		public float stopRate = 5.0f; //Rate of deceleration when player opposes movement above sliding speed in units per second per second
	}
	public Sliding slide = new Sliding();
	
	[System.Serializable]
	public class Jumping {
		public float verticalJumpImpulse = 5.0f; //Players vertical change in speed when jumping up in units per second
		public float directionalJumpImpulse = 3.5f; //Players horizontal change in speed when jumping in a direction in units per second
		public float directionalJumpVerticalImpulse = 4.0f; //Players vertical change in speed when jumping in a direction in units per second
		public float earlyJumpPenalty = 0.2f; //Players horizontal loss of speed for jumping early as a fraction
	}
	public Jumping jump = new Jumping();
	
	// Use this for initialization
	void Start () {
		character = GetComponent<CharacterController>();
		Vector3 euler = transform.eulerAngles;
		control.look = new Vector2(euler.x, euler.y);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 velocity = character.velocity;
		velocity -= character.velocity * move.airFriction * Time.deltaTime;
		velocity.y -= move.gravity * Time.deltaTime;
		
		transform.eulerAngles = new Vector3(0.0f, control.look.y, 0.0f);
		
		if(character.isGrounded) {
			velocity.y = 0;
			
			if(jumpedEarly) {
				velocity *= (1 - jump.earlyJumpPenalty);
				jumpedEarly = false;
			}
			
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
			if(control.jump) {
				velocity -= Mathf.Clamp(Vector3.Dot(velocity, worldMove) / velocity.magnitude, -1f, 0f) * worldMove;
				
				float directionality = control.move.magnitude;
				velocity += (1 - directionality) * Vector3.up * jump.verticalJumpImpulse;
				velocity += directionality * (Vector3.up * jump.directionalJumpVerticalImpulse + worldMove * jump.directionalJumpImpulse);
				
				control.jump = false;
			}
		} else if(control.jump) {
			jumpedEarly = true;
		}
		
		character.Move(velocity * Time.deltaTime);
		Debug.Log(velocity.magnitude);
	}
}
