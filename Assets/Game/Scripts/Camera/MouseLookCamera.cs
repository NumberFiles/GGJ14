using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller))]
public class MouseLookCamera : MonoBehaviour {
	Controller control;
	public Camera attachedCamarea;

	// Use this for initialization
	void Start () {
		control = GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
		attachedCamarea.transform.eulerAngles = new Vector3(control.input.look.x, control.input.look.y, 0.0f);
	}
}
