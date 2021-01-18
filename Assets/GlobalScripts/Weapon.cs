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

	public int flags = 0;
	public string[] entities = new string[] { "Player", "Enemies" };

	public GameObject model;
	public Transform bulletOrigin;
	public GameObject bullet;

	public Vector2Int bulletCount;
	public float bulletDensity = 1;

	public bool holdToFire;
	public float firingTime;
	public float shotSpeed;
	public float range;
	public int damage;

	public int maxAmmo;
	public int maxClip;

	public int ammo;
	public int clip;


}
