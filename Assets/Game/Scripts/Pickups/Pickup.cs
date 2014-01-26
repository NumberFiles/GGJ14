using UnityEngine;
using System.Collections;

public abstract class Pickup : MonoBehaviour {
	public bool playerOnly = true;
	
	// Use this for initialization
	void Start () {
		this.tag = "Pickup";
	}
	 
	public abstract void Grab(Character character);
}
