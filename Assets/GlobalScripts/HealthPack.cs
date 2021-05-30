using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
	public int health = 2;
	public AudioClip healthSound;

	private void Start()
	{
		if (GetComponent<AudioSource>() == null) return;
		GetComponent<AudioSource>().clip = healthSound;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

			if (playerHealth.currentHealth == playerHealth.maxHealth) return;
			
			Debug.Log("TakeDamage: " + -health);
			playerHealth.TakeDamage(-health);

			if (GetComponent<AudioSource>() != null)
			{
				GetComponent<AudioSource>().volume = JsonIO.playerSettings.vol_SoundFX;
				GetComponent<AudioSource>().Play();
			}

			transform.GetChild(0).gameObject.SetActive(false);
			
		}
	}
}
