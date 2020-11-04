﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
	public Slider healthSlider;

	public float immunityWindow = 0.1f;

	private protected override void Start()
	{
		base.Start();

		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;
	}

	float endTime;
	public override void TakeDamage(int damage)
	{
		if (Time.time >= endTime)
		{
			base.TakeDamage(damage);
			healthSlider.value = currentHealth;
			
			endTime = immunityWindow + Time.time;
		}
	}
}