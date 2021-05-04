using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : Health
{
	Slider healthSlider;

	public float immunityWindow = 0.1f;

	public static int startingHealth;

	public static bool loadedFromSave;

	Animator hertEffect;

	private float loadindImmunityEndTime;

	private protected override void Start()
	{

		healthSlider = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("HealthBar").GetComponent<Slider>();

		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;

		hertEffect = GetComponentInChildren<Animator>();

		loadindImmunityEndTime = Time.time + 2;

		if (loadedFromSave)
		{
			loadedFromSave = false;
			currentHealth = QuickSave.mostRecentLoad.health;
			if (currentHealth > maxHealth)
				currentHealth = maxHealth;

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
		// Brief immunity window
		if (Time.time >= endTime					// immunity from regular damage,
			&& Time.time >= loadindImmunityEndTime	// immunity from loading in
			&& damage < 10)							// only if damage is less than amount (stops bypassing death pits)
		{
			base.TakeDamage(damage);
			healthSlider.value = currentHealth;

			endTime = immunityWindow + Time.time;

			if (damage > 0)
			{
				hertEffect.SetTrigger("Play");
			}
		}
	}

	// Added to the unity event
	public void DeathEffect()
	{
		FindObjectOfType<DeathEffectController>().Play();
	}
}
