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

	private protected virtual void Start()
	{
		currentHealth = maxHealth;
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

		if (currentHealth <= 0)
		{
            isDead = true;
		}

		OnHit.Invoke();
	}
}
