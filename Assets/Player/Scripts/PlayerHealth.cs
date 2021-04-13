using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
	Slider healthSlider;

	public float immunityWindow = 0.1f;

	public static int startingHealth;

	public static bool loadedFromSave;

	private protected override void Start()
	{

		healthSlider = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("HealthBar").GetComponent<Slider>();

		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;

		if (loadedFromSave)
		{
			loadedFromSave = false;
			currentHealth = Checkpoints.mostRecentSave.health;
			healthSlider.value = currentHealth;
		}
		else
		{
			base.Start();
		}
	}

	float endTime;
	// Override to give immunity for brief time
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
