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

	public bool[] flags;

	public GameObject model;
	public Transform bulletOrigin;
	public GameObject bullet;
	public ParticleSystem muzzleFlash;

	public Vector2Int bulletCount;
	public float bulletDensity = 1;

	public bool holdToFire;
	public float firingTime;
	public float shotSpeed;
	public float range;
	public int damage;
	public float dmgMult;

	public int maxAmmo;
	public int maxClip;

	public int startingAmmo;
	public int startingClip;

	public int ammo;
	public int clip;

}
