using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(Controller))]
public class CharacterMotor : MonoBehaviour {
	protected CharacterController character;
	protected Controller control;
	
	protected Vector3 unappliedImpulse = Vector3.zero;
	protected bool jumpedEarly = false;
	protected float delayJump = 0.0f;
	public bool doJump {
		get { return control.input.jump && delayJump <= 0.0f; }
		set { control.input.jump = value; }
	}
	
	[System.Serializable]
	public class Movement {
		public float runSpeed = 10.0f; //Running speed in units per second
		public float sqrRunSpeed { get { return runSpeed * runSpeed; } }
		public float groundFriction = 45.0f; //Deceleration of grounded player going above run speed in units per second per second
		public float drag = 1 / 500.0f; //Players drag in the air. Multiply by 1/2 velocity squared to get deceleration
		public float airAcceleration = 2.5f; //Acceleration of player in the air
		public float gravity = 14f;
	}
	public Movement move = new Movement();
	
	[System.Serializable]
	public class Sliding {
		public float slidingSpeed = 12.1f; //Running speed in units per second
		public float sqrSlidingSpeed { get { return slidingSpeed * slidingSpeed; } }
	}
	public Sliding slide = new Sliding();
	
	[System.Serializable]
	public class Jumping {
		public float verticalJumpImpulse = 8.0f; //Players vertical change in speed when jumping up in units per second
		public float directionalJumpImpulse = 3f; //Players horizontal change in speed when jumping in a direction in units per second
		public float directionalJumpVerticalImpulse = 5.6f; //Players vertical change in speed when jumping in a direction in units per second
		public float earlyJumpPenalty = 0.1f; //Players horizontal loss of speed for jumping early as a fraction
	}
	public Jumping jump = new Jumping();
	
	[System.Serializable]
	public class WallRunning {
		public float range = 0.2f; //Minimum ratio of gravity that must be applied when wallrunning and rising
		public float minRisingGravity = 0.75f; //Minimum ratio of gravity that must be applied when wallrunning and rising
		public float risingGravityReductionSlope = 0.075f; //Rate of gravity reduction increase
		public float minFallingGravity = 0.5f;  //Minimum ratio of gravity that must be applied when wallrunning and falling
		public float fallingGravityReductionSlope = 0.075f; //Minimum ratio of gravity that must be applied when wallrunning and rising
		public float friction = 0.0f; //Deceleration of grounded player going above run speed in units per second per second
		public float momentumTransfer = 0.75f;
		public float jumpDirectionality = 0.75f;
		public float jumpVerticality = 0.3f;
	}
	public WallRunning wallrun = new WallRunning();

	protected RaycastHit wallRunFooting = new RaycastHit();
	public bool isWallRunning {
		get { return wallRunFooting.collider != null; }
	}
	public float wallRunning {
		get {
			return isWallRunning ? Vector3.Dot(-wallRunFooting.normal, transform.right) :
					0.0f;
		}
	}
	public void WallRun(RaycastHit footing) {
		wallRunFooting = footing;
	}
	public void StopWallRunning() {
		wallRunFooting = new RaycastHit();
	}
	
	// Use this for initialization
	void Start () {
		character = GetComponent<CharacterController>();
		control = GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(delayJump > 0.0f)
			delayJump -= Time.deltaTime;
		
		Vector3 velocity = character.velocity;
		
		//apply drag
		velocity -= velocity.normalized * Mathf.Clamp(velocity.sqrMagnitude/2 * move.drag * Time.deltaTime, 0.0f, velocity.magnitude);
		
		transform.eulerAngles = new Vector3(0.0f, control.input.look.y, 0.0f);

		Vector3 worldMove = new Vector3(control.input.move.x, 0, control.input.move.y);
		worldMove = transform.rotation * worldMove;
		
		if(character.isGrounded) {
			StopWallRunning();
			velocity.y = 0;
			
			if(jumpedEarly) {
				velocity *= (1 - jump.earlyJumpPenalty);
				jumpedEarly = false;
			}
			
			//calculate our movement input direction in world space
			if(velocity.sqrMagnitude <= slide.sqrSlidingSpeed) {
				velocity = move.runSpeed * worldMove;
			} else {
				Vector3 velDir = velocity.normalized;
				Vector3 accel = Vector3.zero;
				accel -= velDir * move.groundFriction;
				velocity += accel * Time.deltaTime;
			}
			if(doJump) {
				float directionality = control.input.move.magnitude;
				velocity += (1 - directionality) * Vector3.up * jump.verticalJumpImpulse;
				velocity += directionality * (Vector3.up * jump.directionalJumpVerticalImpulse + worldMove * jump.directionalJumpImpulse);
				
				control.input.jump = false;
			}
		} else {
			if(control.input.wallRun) {
				Vector3 hVel = new Vector3(velocity.x, 0.0f, velocity.z);
				Vector3 side = new Vector3(-transform.forward.z, 0.0f, transform.forward.x).normalized;
				Vector3 foot = transform.position + character.center - Vector3.up * (character.height*0.5f);
				RaycastHit info1;
				RaycastHit info2;
				bool hit1 = Physics.SphereCast(foot, character.radius * 0.75f, side, out info1, character.radius * 0.25f + wallrun.range);
				bool hit2 = Physics.SphereCast(foot, character.radius * 0.75f, -side, out info2, character.radius * 0.25f + wallrun.range);
				hit1 = hit1 && info1.collider.tag == "Structure";
				hit2 = hit2 && info2.collider.tag == "Structure";
				if(hit1 || hit2) {
					bool left = hit1 && (!hit2 || info1.distance < info2.distance);
					side = -(left ? info1 : info2).normal;
					
					if(!isWallRunning) {
						float sideSpeed = Mathf.Max(Vector3.Dot(hVel, side), 0.0f);
						Vector3 forward = hVel - sideSpeed * side;
						velocity += forward.normalized * sideSpeed * wallrun.momentumTransfer;
					}
					
					WallRun(left ? info1 : info2);
					//apply friction
					if(wallrun.friction * Time.deltaTime > velocity.magnitude)
						velocity = Vector3.zero;
					else {
						Vector3 velDir = velocity.normalized;
						Vector3 accel = Vector3.zero;
						accel -= velDir * wallrun.friction;
						velocity += accel * Time.deltaTime;
					}
					
					//apply gravity
					float minGravity = velocity.y > 0 ? wallrun.minRisingGravity : wallrun.minFallingGravity;
					float gReductSlope = velocity.y > 0 ? wallrun.risingGravityReductionSlope : wallrun.fallingGravityReductionSlope;
					
					//not gonna explain why this works
					//just google or wulfrumalpha 0.25/(1 + 0.25 - 1/(x*0.075 + 1)) to see curve
					//look at x >= 0
					float reductionFactor = minGravity/(1 + minGravity - 1/(hVel.magnitude*gReductSlope + 1));
					velocity.y -= move.gravity * Time.deltaTime * reductionFactor;
					if(doJump) {
						velocity += wallrun.jumpVerticality * Vector3.up * jump.verticalJumpImpulse;
						velocity += wallrun.jumpDirectionality * (Vector3.up * jump.directionalJumpVerticalImpulse - side * jump.directionalJumpImpulse);
						
						control.input.jump = false;
						delayJump = 0.5f;
					}
				} else {
					StopWallRunning();
				}
			} else {
				StopWallRunning();
			}
			if (!isWallRunning){
				velocity.y -= move.gravity * Time.deltaTime;
				
				Vector3 p1 = transform.position + character.center + Vector3.up * (-character.height*0.5f);
				Vector3 p2 = p1 + Vector3.up * character.height;
				Vector3 acceleratedVelocity = velocity + worldMove * move.airAcceleration * Time.deltaTime;
				if(!Physics.CapsuleCast(p1, p2, character.radius, acceleratedVelocity, acceleratedVelocity.magnitude * Time.deltaTime))
					velocity = acceleratedVelocity;
				if(doJump) {
					jumpedEarly = true;
				}
			}
		}
		
		velocity += unappliedImpulse;
		unappliedImpulse = Vector3.zero;

		Vector3 start = transform.position;
		character.Move(velocity * Time.deltaTime);

		/*Quick dirty and ugly fix for deadstops on meshes
		Basically see if moving the player 0.01f perindicular to velocity and attempting
		move again results in a noticable difference*/
		//TODO: Remove this hack and find a good solution
		float desiredSpeed = velocity.magnitude;
		float speed = character.velocity.magnitude;
		if((velocity.x != 0 || velocity.z != 0) && (desiredSpeed - speed)/desiredSpeed > 0.25f) {
			float speed0 = speed * 1.25f;
			float speed1 = 0f;
			float speed2 = 0f;

			Vector3 side = new Vector3(-velocity.z, 0.0f, velocity.x).normalized * 0.01f;

			Vector3 p1 = start + character.center + Vector3.up * (-character.height*0.5f);
			Vector3 p2 = p1 + Vector3.up * character.height;
			// Cast character controller shape 10 meters forward, to see if it is about to hit anything

			if(!Physics.CapsuleCast(p1, p2, character.radius, side, 0.015f)) {
				transform.position = start + side;
				character.Move(velocity * Time.deltaTime);
				speed1 = character.velocity.magnitude;
			}
			

			if(!Physics.CapsuleCast(p1, p2, character.radius, -side, 0.015f)) {
				transform.position = start - side;
				character.Move(velocity * Time.deltaTime);
				speed2 = character.velocity.magnitude;
			}

			if(speed1 > speed0 || speed2 > speed0) {
				transform.position = start + (speed1 > speed2 ? side : -side);
				character.Move(velocity * Time.deltaTime);
			} else {
				transform.position = start;
				character.Move(velocity * Time.deltaTime);
			}
		}
	}
	
	public void ApplyImpulse(Vector3 impulse) {
		unappliedImpulse += impulse;
	}
}
