using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
	public bool SpawnModel;
    public Weapon weapon;

	//public AudioClip pickUPSound;


	private void OnValidate()
	{
		if (SpawnModel)
		{
			Instantiate(weapon.model, transform).name = weapon.name;
			SpawnModel = false;
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
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

			Destroy(gameObject);
		}
	}


}
