using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Health : MonoBehaviour
{
	[HideInInspector]
    public int currentHealth;
    public int maxHealth;
	public bool isDead { get; private set; }

	public bool dontDestroyOnDeath;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	private void Update()
	{
		if (!dontDestroyOnDeath && isDead)
		{
			Destroy(gameObject);
		}
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

        if (currentHealth <= 0)
		{
            isDead = true;
		}
	}
}
