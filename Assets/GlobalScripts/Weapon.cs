using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
	HitScan,
	Projectile
}


[CreateAssetMenu(fileName = "WeaponData", menuName ="Weapon", order = 1)]
public class Weapon : ScriptableObject
{
	public string Name;
	public WeaponType weaponType;
	public LayerMask hitLayers;

	public Weapon(LayerMask hitLayers)
	{
		this.hitLayers = hitLayers;
	}

	public GameObject model;
	public GameObject bullet;
	public ParticleSystem muzzleFlash;

	public AudioClip fireAudio;
	public AudioClip reloadAudio;

	public int bulletIndex;

	public Vector2Int bulletCount;
	public float bulletDensity = 1;

	public bool holdToFire;
	public float firingTime;
	public float reloadTime;
	public float shotSpeed;
	public float range;
	public int damage;
	public float dmgMult;

	public bool isAmmoInf;
	public int maxAmmo;
	public int startingAmmo;

	public bool isClipInf;
	public int maxClip;
	public int startingClip;

	public int ammo;
	public int clip;


	public Weapon()
	{
		Name = "";
		weaponType = WeaponType.HitScan;
		hitLayers = new LayerMask();

		model = null;
		bullet = null;
		muzzleFlash = null;

		fireAudio = null;
		reloadAudio = null;

		bulletIndex = 0;
		bulletCount = new Vector2Int(0, 0);
		bulletDensity = 0;

		holdToFire = false;
		firingTime = 0;
		reloadTime = 0;
		shotSpeed = 0;
		range = 0;
		damage = 0;
		dmgMult = 0;

		isAmmoInf = false;
		maxAmmo = 0;
		startingAmmo = 0;

		isClipInf = false;
		maxClip = 0;
		startingClip = 0;

		ammo = 0;
		clip = 0;
	}

	
	public Transform GetBulletOrigin(Transform sceneObject)
	{
		if (bulletIndex < sceneObject.childCount && bulletIndex >= 0)
		{
			return sceneObject.GetChild(bulletIndex);
		}
		else
		{
			Debug.Log("Index out of bounds, returning null");
			return null;
		}
	}
}
