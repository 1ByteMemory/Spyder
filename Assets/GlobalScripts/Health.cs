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
		if (transform.name != "Player")
			Debug.Log("Taking Damage");
		
		currentHealth -= damage;
		OnHit.Invoke();

		if (currentHealth <= 0 && !isDead)
		{
			Debug.Log("Dead");
			isDead = true;
			OnDeath.Invoke();
		}
	}
}
