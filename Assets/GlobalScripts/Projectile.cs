using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public Weapon weapon;
	public int layer;
	public string ownerTag;
	private float endTime = -1;
	private Vector3 pos;


	private void OnEnable()
	{
		pos = transform.position;
	}

	private void Update()
	{
		if (weapon != null)
		{
			// Calulate the distance without useing Vector3.Distance
			float xD = pos.x - transform.position.x;
			float yD = pos.y - transform.position.y;
			float zD = pos.z - transform.position.z;
			float distance = xD * xD + yD * yD + zD * zD;

			// Check against range^2 instead of useing sqrRoot on distance
			if (distance >= weapon.range * weapon.range)
			{
				Destroy(gameObject);
			}
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger) return;

		// On hit player and not shot from player
		if (other.CompareTag("Player") && ownerTag != "Player")
		{
			if (layer == GameManager.activeLayer)
			{
				// If in same layer, deal regular damge
				DealDamage(other, weapon.damage);
			}
			else
			{
				// If not in same layer, deal less damge
				DealDamage(other, Mathf.FloorToInt(weapon.damage * weapon.dmgMult));
			}
		}


		// On hit anything, but the one who shot it, other projectiles and objects not in the active layer
		if (!other.CompareTag(ownerTag) && !other.CompareTag("Projectile"))
		{
			DealDamage(other, weapon.damage);
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
