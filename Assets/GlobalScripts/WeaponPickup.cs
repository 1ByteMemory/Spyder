using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
	public bool SpawnModel;
    public Weapon weapon;

	//public AudioClip pickUPSound;
	public AudioClip pickUpSound;
	private AudioSource src;

	private void OnValidate()
	{
		if (SpawnModel)
		{
			Instantiate(weapon.model, transform).name = weapon.name;
			SpawnModel = false;
		}
		if (GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent<AudioSource>();
		}
	}

	private void Start()
	{
		src = GetComponent<AudioSource>();
		src.playOnAwake = false;
		src.loop = false;
	}

	private bool pickedUp;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && pickedUp == false)
		{
			pickedUp = true;
			PlayerWeapon pw = other.GetComponent<PlayerWeapon>();

			if (pw.weapons.Contains(weapon))
			{
				WeaponBehaviour.RefillAmmo(pw.GetWeapon(weapon.name), weapon.maxAmmo / 2);
			}
			else
			{
				pw.weapons.Add(weapon);
				pw.InstantiateWeapon(pw.gunViewModel, weapon);
				pw.CycleWeapons(pw.weapons.Count - 1, true);
			}

			// Play pick up sound
			src.volume = JsonIO.playerSettings.vol_SoundFX;
			src.clip = pickUpSound;
			src.Play();

			// Destroy child objects
			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}
}
