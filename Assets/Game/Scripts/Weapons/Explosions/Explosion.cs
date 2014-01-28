using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public float coreImpulse = 5.0f;
	public float edgeImpulse = 1.0f;
	public float coreDamage = 50.0f;
	public float edgeDamage = 5.0f;
	public float coreRadius = 0.5f;
	public float edgeRadius = 3.0f;
	
	public float initialScale = 0.5f;
	public float finalScale = 5.0f;
	public float lifeTime = 0.15f;
	public Object spawnOnDestroy = null;
	
	protected float liveTime = 0.0f;

	// Use this for initialization
	void Start () {
		Collider[] hits = Physics.OverlapSphere(transform.position, edgeRadius);
		foreach(Collider collider in hits) {
			Character character = collider.GetComponent<Character>();
			CharacterMotor motor = collider.GetComponent<CharacterMotor>();
			if(motor != null || character != null || collider.attachedRigidbody != null)
			{
				Vector3 to = (collider.attachedRigidbody ? collider.attachedRigidbody .centerOfMass : collider.transform.position) - transform.position;
				if(to.Equals(Vector3.zero))
					to = Vector3.up;
				float distanceToCenter = to.magnitude;
				to.Normalize();
				RaycastHit raycastHit;
				float forceRatio = 1.0f;
				if(collider.Raycast(new Ray(transform.position, to), out raycastHit, distanceToCenter * 1.1f))
					forceRatio = Mathf.Clamp((edgeRadius - raycastHit.distance) / (edgeRadius - coreRadius), 0.0f, 1.0f);
				if(character != null) {
					character.healthPoints -= coreDamage * forceRatio + edgeDamage * (1 - forceRatio);
				}
				if(motor != null) {
					motor.ApplyImpulse((coreImpulse*forceRatio + edgeImpulse*(1 - forceRatio)) * to);
				} else if(collider.attachedRigidbody != null) {
					collider.attachedRigidbody.AddForce((coreImpulse*forceRatio + edgeImpulse*(1 - forceRatio)) * to, ForceMode.Impulse);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(liveTime > lifeTime)
			Destroy(gameObject);
		else
			transform.localScale = Vector3.one * (coreRadius + (edgeRadius - coreRadius) * (liveTime / lifeTime));
		liveTime += Time.deltaTime;
	}
}
