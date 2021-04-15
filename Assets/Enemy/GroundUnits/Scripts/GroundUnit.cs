using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundUnit : SearchAndDestory
{

	public float reloadTime;

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
	}

	protected override void Attack()
	{
		base.Attack();
		
	}
}
