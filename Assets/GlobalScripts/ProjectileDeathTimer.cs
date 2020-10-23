﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileDeathTimer : MonoBehaviour
{
	public float lifeTime;
	float endTime;
	public string ownerTag;

	private void OnEnable()
	{
		endTime = lifeTime + Time.time;
	}

	private void Update()
	{
		if (Time.time >= endTime)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(ownerTag) && !other.CompareTag("Projectile"))
		{
			// Play explosion animation

			Health health = other.GetComponent<Health>();
			if (health != null)
			{
				health.TakeDamage(1);
			}

			Destroy(gameObject);
		}
	}
}
