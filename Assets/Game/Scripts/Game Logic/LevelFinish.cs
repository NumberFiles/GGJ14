using UnityEngine;
using System.Collections;

public class LevelFinish : MonoBehaviour {
	static float finishTime = 0.0f;
	static float startTime = 0.0f;
	static bool finished = false;
	public GUIStyle style = new GUIStyle();
	public bool thisLineDrawsTime = false;

	// Use this for initialization
	void Start () {
		startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider) {
		if(!finished && collider.gameObject.tag == "Player") {
			finishTime = Time.timeSinceLevelLoad;
			finished = true;
		}
	}
	
	void OnGUI() {
		if(thisLineDrawsTime && finished) {
			GUI.Label(new Rect(0, 0, 1920, 1080), "Finish Time: " + (finishTime - startTime) + "s", style);
		}
	}
}
