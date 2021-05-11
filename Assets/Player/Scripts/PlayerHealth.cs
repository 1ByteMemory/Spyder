using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : Health
{
	TextMeshProUGUI healthSlider;

	public float immunityWindow = 0.1f;

	public static int startingHealth;

	public static bool loadedFromSave;

	Animator hertEffect;

	private float loadindImmunityEndTime;

	private protected override void Start()
	{
		healthSlider = FindObjectOfType<GameManager>().PlayerHUD.GetComponent<PlayerHUD>().health;

		healthSlider.text = maxHealth.ToString();

		hertEffect = GetComponentInChildren<Animator>();

		loadindImmunityEndTime = Time.time + 2;

		if (loadedFromSave)
		{
			loadedFromSave = false;
			currentHealth = QuickSave.mostRecentLoad.health;
			if (currentHealth > maxHealth)
				currentHealth = maxHealth;

			healthSlider.text = currentHealth.ToString();
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
			healthSlider.text = currentHealth.ToString();

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
