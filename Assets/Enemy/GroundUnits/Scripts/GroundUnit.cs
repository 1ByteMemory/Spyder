using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundUnit : SearchAndDestory
{

	[Header("Weapons")]
	public WeaponBehaviour weapon;
	public float reloadTime;

	public int normalDamage = 2;
	public int modifiedDamage = 1;


	protected override void Start()
	{
		base.Start();
	}

	float endStunTime;
	protected override void Update()
	{
		base.Update();

		if (isStunned && Time.time >= endStunTime)
		{
			isStunned = false;
		}

	}

	public override void EnemyDefeated()
	{
		base.EnemyDefeated();
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

		if (!isStunned)
		{
			if (Time.time >= fireingTime)
			{
				/*
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
					ProjectileDeathTimer projectileDeathTimer = projectile.GetComponent<ProjectileDeathTimer>();
					
					projectileDeathTimer.ownerTag = "Enemy";
					projectileDeathTimer.layer = gameObject.layer;
					projectileDeathTimer.normalDamage = normalDamage;
					projectileDeathTimer.modifiedDamage = modifiedDamage;

					weapon.FireProjectile(projectile, (player.position - transform.position).normalized);
					
				}
				*/
			}
		}
	}
	

	/*
	public override void Stun()
	{
		base.Stun();

		endStunTime = Time.time + stunnedTime;
	}
	*/
}
