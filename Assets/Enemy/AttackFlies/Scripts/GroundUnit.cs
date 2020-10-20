using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundUnit : SearchAndDestory
{
	public WeaponBehaviour weapon;
	public float reloadTime;

	protected override void Start()
	{
		base.Start();
	}
	protected override void Update()
	{
		base.Update();
	}

	protected override void Idle()
	{
		base.Idle();
		
	}


	protected override void Search()
	{
		base.Search();

		// Go to player
		

	}

	float fireingTime;
	float reloadEndTime;
	protected override void Attack()
	{
		base.Attack();
		
		if (Time.time >= fireingTime)
		{
			fireingTime = Time.time + weapon.fireingTime;

			if (weapon.Clip == 0)
			{
				reloadEndTime = Time.time + reloadTime;
				weapon.Clip = weapon.maxClipSize;
			}

			if (weapon.Clip > 0 && Time.time > reloadEndTime)
			{
				weapon.Clip--;

				GameObject projectile = Instantiate(weapon.projectile, transform.position, new Quaternion());
				projectile.GetComponent<ProjectileDeathTimer>().ownerTag = "Enemy";

				weapon.FireProjectile(projectile, (Player.position - transform.position).normalized);

			}
		}
	}
}
