using System.Collections;
using System.Collections.Generic;
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


	public bool dimminsionHitOnly = false;
	public Dimension dimension;


	private protected virtual void Start()
	{
		currentHealth = maxHealth;

	}

	private protected virtual void Update()
	{
		if (!dontDestroyOnDeath && isDead)
		{
			//gameObject.SetActive(false);
			Destroy(gameObject, Time.deltaTime);
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
			if (GameManager.currentDimension == dimension)
			{
				Damage(damage);
			}
		}
	}

	private void Damage(int damage)
	{
		
		currentHealth -= damage;
		if (damage > 0)
		{
			OnHit.Invoke();
		}

		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}

		if (currentHealth <= 0 && !isDead)
		{
			isDead = true;
			OnDeath.Invoke();
		}
	}
}
