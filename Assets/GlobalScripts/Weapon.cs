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
	public int weaponTypeIndex;
	public WeaponType weaponType;
	public LayerMask hitLayers;

	public Weapon(LayerMask hitLayers)
	{
		this.hitLayers = hitLayers;
	}

	public bool[] flags;

	public GameObject model;
	public GameObject bullet;
	public ParticleSystem muzzleFlash;
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
	public int bulletIndex;

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
