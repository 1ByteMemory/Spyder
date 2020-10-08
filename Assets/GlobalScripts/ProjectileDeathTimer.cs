using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeathTimer : MonoBehaviour
{
	public float lifeTime;
	float endTime;

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
		// Play explosion animation


		Destroy(gameObject);
	}
}
