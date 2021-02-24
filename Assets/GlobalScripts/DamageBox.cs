using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageBox : MonoBehaviour
{
	public int damagePerSecond;

	float endTime;

	private void OnCollisionStay(Collision collision)
	{
		if (collision.transform.CompareTag("Player") && Time.time >= endTime)
		{
			endTime = Time.time + 0.2f;

			collision.gameObject.GetComponent<Health>().TakeDamage(damagePerSecond);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && Time.time >= endTime)
		{
			endTime = Time.time + 0.2f;

			other.GetComponent<Health>().TakeDamage(damagePerSecond);
		}
	}

}
