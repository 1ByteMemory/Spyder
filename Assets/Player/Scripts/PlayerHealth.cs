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

	private protected override void Start()
	{

		healthSlider = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("HealthBar").GetComponent<Slider>();

		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;

		hertEffect = GetComponentInChildren<Animator>();

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
		if (Time.time >= endTime)
		{
			base.TakeDamage(damage);
			healthSlider.value = currentHealth;
			
			endTime = immunityWindow + Time.time;

			hertEffect.SetTrigger("Play");
		}
	}

	// Added to the unity event
	public void DeathEffect()
	{
		FindObjectOfType<DeathEffectController>().Play();
	}
}
