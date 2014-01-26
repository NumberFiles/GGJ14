using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	public class ControlInput {
		public Vector2 move = Vector2.zero;
		public Vector2 look = Vector2.zero;
		public bool jump = false;
		public bool wallRun = false;
		public int weapon = 0;
		public bool fire = false;
	}
	public ControlInput input = new ControlInput();

	// Use this for initialization
<<<<<<< HEAD
	void Start () {
=======
	protected virtual void Start () {
>>>>>>> origin/master
		Vector3 euler = transform.eulerAngles;
		input.look = new Vector2(euler.x, euler.y);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
