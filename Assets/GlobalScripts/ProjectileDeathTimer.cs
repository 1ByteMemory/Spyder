using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileDeathTimer : MonoBehaviour
{
	public float lifeTime;
	float endTime;
	public string ownerTag;
	public int layer;

	public int normalDamage;
	public int modifiedDamage;

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

		// On hit player and not shot from player
		if (other.CompareTag("Player") && ownerTag != "Player")
		{
			if (layer == GameManager.activeLayer)
			{
				// If in same layer, deal regular damge
				DealDamage(other, normalDamage);
			}
			else
			{
				// If not in same layer, deal less damge
				DealDamage(other, modifiedDamage);
			}
		}


		// On hit anything, but the one who shot it, other projectiles and objects not in the active layer
		if (!other.CompareTag(ownerTag) && !other.CompareTag("Projectile") && GameManager.activeLayer == other.gameObject.layer)
		{

			// Play explosion animation

			DealDamage(other, normalDamage);
		}
	}

	void DealDamage(Collider other, int dmg)
	{
		Health health = other.GetComponentInParent<Health>();
		if (health != null)
		{
			health.TakeDamage(dmg);
		}

		Destroy(gameObject);

	}
}
