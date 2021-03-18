using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DigitalUnit : SearchAndDestory
{

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
		
	}
}
