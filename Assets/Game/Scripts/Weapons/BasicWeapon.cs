using UnityEngine;
using System.Collections;

public class BasicWeapon : Weapon {
	public Projectile projectile;
	public float spawnAheadBy = 1.0f;
	public float fireDelay = 1.0f;
	
	bool isDrawn = false;
	
	protected virtual void FixedUpdate() {
		fireDelay -= Time.deltaTime;
	}

	public override float Draw(){
		if(!isDrawn) {
			gameObject.SetActive(true);
			isDrawn = true;
		}
		return 0;
	}
	
	public override float Holster(){
		if(isDrawn) {
			gameObject.SetActive(false);
			isDrawn = false;
		}
		return 0;
	}
	
	public override bool Fire(Vector3 direction){
		Instantiate(projectile, transform.position + direction * spawnAheadBy, Quaternion.LookRotation(direction));
		return false;
	}
}
