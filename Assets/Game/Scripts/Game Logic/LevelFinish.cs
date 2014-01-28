using UnityEngine;
using System.Collections;

public class LevelFinish : MonoBehaviour {
	static float startTime = 0.0f;
	static float currentTime = float.NaN;
	static float lastTime = float.NaN;
	static float bestTime = float.NaN;
	static bool finished = false;
	public GUIStyle style = new GUIStyle();
	public bool thisLineDrawsTime = false;

	static string bestTimePrefName {
		get { return "Best Time - " + Application.loadedLevelName + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
	}

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.HasKey(bestTimePrefName))
			bestTime = PlayerPrefs.GetFloat(bestTimePrefName);
		if(!float.IsNaN(currentTime))
			lastTime = currentTime;
		currentTime = float.NaN;
		startTime = Time.timeSinceLevelLoad;
		finished = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider) {
		if(!finished && collider.gameObject.tag == "Player") {
			currentTime = Time.timeSinceLevelLoad - startTime;
			if(float.IsNaN(bestTime) || currentTime <  bestTime) {
				bestTime = currentTime;
				PlayerPrefs.SetFloat(bestTimePrefName, bestTime);
			}
			finished = true;
		}
	}
	
	void OnGUI() {
		if(thisLineDrawsTime) {
			GUI.Label(new Rect(0, 0, 1920, 1080), "Current Time: " + (float.IsNaN(currentTime) ? (Time.timeSinceLevelLoad - startTime) : currentTime) + "s", style);
			if(!float.IsNaN(lastTime)) {
				GUI.Label(new Rect(0, style.lineHeight, 1920, 1080), "Last Time: " + lastTime + "s", style);
				if(!float.IsNaN(bestTime))
					GUI.Label(new Rect(0, style.lineHeight * 2f, 1920, 1080), "Best Time: " + bestTime + "s", style);
			} else if(!float.IsNaN(bestTime)) {
				GUI.Label(new Rect(0, style.lineHeight, 1920, 1080), "Best Time: " + bestTime + "s", style);
			}
		}
	}
}
