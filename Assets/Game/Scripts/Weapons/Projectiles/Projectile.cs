using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Projectile : MonoBehaviour {
	public float speed = 50.0f;
	public float damage = 10.0f;
	public float impulse = 0.0f;
	public Object spawnOnDestroy = null;

	// Use this for initialization
	void Start () {
		this.rigidbody.velocity = speed * transform.forward;
	}
	
	void OnCollisionEnter(Collision collision) {
		Character character = collision.gameObject.GetComponent<Character>();
		if(character != null) {
			character.healthPoints -= damage;
		}
		if(impulse != 0.0f) {
			CharacterMotor motor = collision.gameObject.GetComponent<CharacterMotor>();
			if(motor != null) {
				motor.ApplyImpulse((motor.transform.position - transform.position) * impulse);
			}
		}
		Destroy(gameObject);
	}
	
	void OnDestroy() {
		if(spawnOnDestroy != null) {
			Instantiate(spawnOnDestroy,transform.position, transform.rotation);
		}
	}
}
