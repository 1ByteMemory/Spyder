using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    private protected override void Start()
	{
		base.Start();
	}

	private protected override void Update()
	{
		base.Update();
	}

	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
	}
}
