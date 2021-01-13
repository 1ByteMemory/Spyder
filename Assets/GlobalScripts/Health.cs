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


	private protected virtual void Start()
	{
		currentHealth = maxHealth;

		bloodParticle = GetComponentInChildren<ParticleSystem>();
	}

	private protected virtual void Update()
	{
		if (!dontDestroyOnDeath && isDead)
		{
			Destroy(gameObject);
		}
	}

	public virtual void TakeDamage(int damage)
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
