using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	[HideInInspector]
    public int currentHealth;
    public int maxHealth;
	public bool isDead { get; private set; }

	public bool dontDestroyOnDeath;

	public UnityEvent OnHit;
	public UnityEvent OnDeath;

	private ParticleSystem bloodParticle;

	public bool dimminsionHitOnly = false;
	public Dimension dimension;


	private protected virtual void Start()
	{
		currentHealth = maxHealth;

		bloodParticle = GetComponentInChildren<ParticleSystem>();
	}

	private protected virtual void Update()
	{
		if (!dontDestroyOnDeath && isDead)
		{
			//gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}

	public virtual void TakeDamage(int damage)
	{
		if (!dimminsionHitOnly)
		{
			Damage(damage);
		}
		else
		{
			if (GameManager.currentActiveDimension == dimension)
			{
				Damage(damage);
			}
		}
	}

	private void Damage(int damage)
	{
		currentHealth -= damage;

		if (bloodParticle != null)
		{
			bloodParticle.Play();
		}

		if (currentHealth <= 0 && !isDead)
		{
			isDead = true;
			OnDeath.Invoke();
		}

		OnHit.Invoke();
	}
}
