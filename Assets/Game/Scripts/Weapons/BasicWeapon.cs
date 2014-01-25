using UnityEngine;
using System.Collections;

public class BasicWeapon : Weapon {
	public Projectile projectile;
	public float spawnAheadBy = 1.0f;
	public float shotDelay = 1.0f;
	public bool fullAuto = false;
	protected float fireDelay = 0.0f;
	
	bool isDrawn = false;
	
	protected virtual void FixedUpdate() {
		if(fireDelay > 0.0f)
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
		if(fireDelay <= 0.0f) {
			Instantiate(projectile, transform.position + direction * spawnAheadBy, Quaternion.LookRotation(direction));
			fireDelay = shotDelay;
		}
		return fullAuto;
	}
}
