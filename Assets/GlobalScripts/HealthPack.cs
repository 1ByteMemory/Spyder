using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
	public int health = 2;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{

			other.GetComponent<PlayerHealth>().TakeDamage(-health);

			gameObject.SetActive(false);
		}
	}
}
