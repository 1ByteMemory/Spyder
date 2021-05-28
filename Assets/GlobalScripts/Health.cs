using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public AudioClip[] deathClip;
	public AudioClip[] hurtClip;
	public AudioSource deathSoundsrc;

	[HideInInspector]
    public int currentHealth;
    public int maxHealth;
	public bool isDead { get; private set; }

	public bool dontDestroyOnDeath;

	public UnityEvent OnHit;
	public UnityEvent OnDeath;


	public bool dimminsionHitOnly = false;
	public Dimension dimension;


	private protected virtual void Start()
	{
		currentHealth = maxHealth;


		if (deathSoundsrc != null)
		{
			//deathSoundsrc.clip = deathClip;
			deathSoundsrc.playOnAwake = false;
			deathSoundsrc.loop = false;
		}
	}

	private protected virtual void Update()
	{
		if (!dontDestroyOnDeath && isDead)
		{
			//gameObject.SetActive(false);
			Destroy(gameObject, Time.deltaTime);
		}
	}

	public void Hurt(bool Dead)
	{
		deathSoundsrc.volume = JsonIO.playerSettings.vol_SoundFX;
		if (Dead)
			deathSoundsrc.clip = deathClip.Length > 0 ? deathClip[Random.Range(0, deathClip.Length)] : null;
		else
			deathSoundsrc.clip = hurtClip.Length > 0 ? hurtClip[Random.Range(0, hurtClip.Length)] : null;


		if (deathSoundsrc.isPlaying) deathSoundsrc.Stop();
		deathSoundsrc.Play();
	}

	public virtual void TakeDamage(int damage)
	{
		if (!dimminsionHitOnly)
		{
			Damage(damage);
		}
		else
		{
			if (GameManager.currentDimension == dimension)
			{
				Damage(damage);
			}
		}
	}

	private void Damage(int damage)
	{
		
		currentHealth -= damage;

		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}

		if (currentHealth <= 0 && !isDead)
		{
			isDead = true;
			OnDeath.Invoke();
		}
		else if (damage > 0 && !isDead)
		{
			OnHit.Invoke();
		}
	}
}
