using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller))]
public class MouseLookCamera : MonoBehaviour {
	Controller control;
<<<<<<< HEAD
	public Camera attachedCamera;
=======
	public Camera attachedCamarea;
>>>>>>> origin/master

	// Use this for initialization
	void Start () {
		control = GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
		attachedCamera.transform.eulerAngles = new Vector3(control.input.look.x, control.input.look.y, 0.0f);
=======
		attachedCamarea.transform.eulerAngles = new Vector3(control.input.look.x, control.input.look.y, 0.0f);
>>>>>>> origin/master
	}
}
