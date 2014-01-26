using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController))]
public class EnemyAIController : Controller {
	
	//Components
	private Transform thePlayerPosition;
	private NavMeshAgent navAgent;
	private CharacterController characterController;
	
	
	//Constants
	private readonly int SEED = 123;
	private readonly int PERCENTAGE_CHANCE_TO_IDLE = 30;
	private readonly float CHASING_DISTANCE = 20.0f;
	private readonly float SHOOTING_DISTANCE = 10.0f;
	
	//Constants - random walks (relative to current position)
	private static float RANDOM_WALK_MAX_SPEED = 5.0f;
	private readonly Vector3 North = new Vector3(RANDOM_WALK_MAX_SPEED, 0.0f, 0.0f);
	private readonly Vector3 East = new Vector3(-1*RANDOM_WALK_MAX_SPEED, 0.0f, 0.0f);
	private readonly Vector3 South = new Vector3(0.0f, 0.0f, RANDOM_WALK_MAX_SPEED);
	private readonly Vector3 West = new Vector3(0.0f, 0.0f, -1*RANDOM_WALK_MAX_SPEED);
	
	
	//variables
	private float distanceToPlayer;
	
	// Use this for initialization
	void Start () {
		thePlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
		navAgent = GetComponent<NavMeshAgent>();
		characterController = GetComponent<CharacterController>();	
	}
	
	// FixedUpdate is called once per game tick
	void FixedUpdate () {
		RandomStep();
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
	
	private void IdleOrRandomStep(int percentageToIdle) {
		int rollToIdle = Random.Range(0, percentageToIdle);
		if (rollToIdle <= percentageToIdle) {
			Idle ();	
		}
		else {
			RandomStep();	
		}
	}
	
	private void Idle() {
		//do nothing
	}
	
	private void RandomStep() {
		UnityEngine.Random.seed = SEED;
		float moveX = Random.Range(-RANDOM_WALK_MAX_SPEED, RANDOM_WALK_MAX_SPEED);
		float moveZ = Random.Range(-RANDOM_WALK_MAX_SPEED, RANDOM_WALK_MAX_SPEED);
		Vector3 randomMovement = new Vector3(moveX, 0, moveZ);
		navAgent.Move(randomMovement); 
	}
}

