using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[HideInInspector]
	public Weapon weapon;
	public int layer;
	public string ownerTag;
	float endTime = -1;


	private void Update()
	{
		if (weapon != null)
		{
			if (endTime == -1)
				endTime = weapon.range + Time.time;

			if (Time.time >= endTime)
			{
				Destroy(gameObject);
			}
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

		//if (other.gameObject.layer == 0 && !other.CompareTag("Projectile"))
		//{
		//	Debug.Log(other.gameObject);
		//	DealDamage(other, weapon.damage);
		//}
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
