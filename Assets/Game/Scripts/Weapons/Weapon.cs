using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {
	public string weaponName = "Unnamed";
	public string weaponSlot = "Default";

	public abstract bool Fire(Vector3 direction, Component owner = null);
	public abstract float Draw();
	public abstract float Holster();
}
