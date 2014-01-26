using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {
	public string weaponName = "Unnamed";
	public string weaponSlot = "Default";
	
<<<<<<< HEAD
	public abstract bool Fire(Vector3 direction);
=======
	public abstract bool Fire(Vector3 direction, Component owner = null);
>>>>>>> origin/master
	public abstract float Draw();
	public abstract float Holster();
}
