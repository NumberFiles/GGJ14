using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
	
	public Waypoint[] waypoints;
	
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (transform.position, 0.25f);
		foreach (Waypoint wp in waypoints) {
			Gizmos.color = Color.magenta;
			Vector3 to = wp.transform.position - transform.position;
			Vector3 side = Quaternion.Euler(0, 90, 0) * to;
			side.Normalize();
			Gizmos.DrawLine(transform.position + side * 0.25f, wp.transform.position + side * 0.25f);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(transform.position + side * 0.25f, to / 4.0f);
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
