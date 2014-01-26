using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class EnemyAIController : Controller {
	
	public Waypoint[] path;
	private Waypoint current;
	private int currentIndex = 0;
	
	//Components
	private Vector3 thePlayerPosition;
	private Transform Destination;
	private CharacterController characterController;
	
	
	//Constants
	private readonly int RANDOMIZER_SEED = 123;
	private readonly int PERCENTAGE_CHANCE_TO_IDLE = 0;
	private readonly float CHASING_DISTANCE = 20.0f;
	private readonly float SHOOTING_DISTANCE = 10.0f;
	private static float WALK_SPEED = 2.0f;
	private static float CLOSE_ENOUGH_TO_WAYPOINT = 2.0f;
	
	
	
	//variables
	private float distanceToPlayer;
	
	// Use this for initialization
	protected override void Start () {
		thePlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
		characterController = GetComponent<CharacterController>();	
		UnityEngine.Random.seed = RANDOMIZER_SEED;
		current = path[currentIndex];
	}
	
	// FixedUpdate is called once per game tick
	void FixedUpdate () {
		IdleOrStepTowardCurrentWaypoint(PERCENTAGE_CHANCE_TO_IDLE);
	/*	distanceToPlayer = CalculateDistanceToPlayer();
		
		if (distanceToPlayer <= SHOOTING_DISTANCE) {
			ShootAtPlayer();
		}
		else if (distanceToPlayer <= CHASING_DISTANCE) {
			ChasePlayer();
		}
		else {
			IdleOrRandomStep(PERCENTAGE_CHANCE_TO_IDLE);
		}
		*/
	}	
	
	private float CalculateDistanceToPlayer() {
		return 0.0f;
	}
	
	private void ShootAtPlayer() {
		
	}
	
	private void ChasePlayer() {
		
	}
	
	private void IdleOrStepTowardCurrentWaypoint(int percentageToIdle) {
		int rollToIdle = Random.Range(0, 100);
		Debug.Log("Roll to Idle: " + rollToIdle);
		if (rollToIdle <= percentageToIdle) {
			Debug.Log("Idle");
			Idle ();	
		}
		else {
			StepTowardCurrentWaypoint();	
		}
	}
	
	private void Idle() {
		//do nothing
	}
	
	private void StepTowardCurrentWaypoint() {
		if (!CurrentWaypointReached()) {
			Vector3 move = current.transform.position - transform.position;
			Vector3 moveAtWalkSpeed = move.normalized * WALK_SPEED;
			characterController.SimpleMove(moveAtWalkSpeed);
		}
		else {
			ChangeToNextWaypointIfAny();				
		}
	}
	
	private bool CurrentWaypointReached() {		
		
		Vector3 difference = current.transform.position - transform.position;
		return  difference.sqrMagnitude < CLOSE_ENOUGH_TO_WAYPOINT;	
	}
	
	private void ChangeToNextWaypointIfAny() {
		Debug.Log("Trying to change waypoint.");
		int nextIndex = currentIndex+1;
		if (nextIndex < path.Length) {
			currentIndex++;		
		}
		else {
			currentIndex = 0;	
		}
		current = path[currentIndex];
		
	}
}

