using UnityEngine;


public class WeaponBehaviour : MonoBehaviour
{
    
    protected int weaponIndex;
    public Weapon[] weapons;

    protected bool isFiring;
    protected bool isReloading;
    
    private float firingEndTime;
    private float reloadingEndTime;

    protected virtual void Start()
	{
		foreach (Weapon item in weapons)
		{
            item.clip = item.startingClip;
            item.ammo = item.startingAmmo;
        }
	}

    protected virtual void InstantiateWeapons(Transform gunTransform)
	{
        foreach (Weapon gun in weapons)
        {
            GameObject _gun = Instantiate(gun.model, gunTransform);

            ParticleSystem bullet = gun.bullet.GetComponent<ParticleSystem>();
            if (bullet != null)
            {
                bullet = Instantiate(gun.bullet, gun.GetBulletOrigin(_gun.transform)).GetComponent<ParticleSystem>();
                var main = bullet.main;
                main.playOnAwake = false;
            }

            ParticleSystem muzzleFlash = gun.muzzleFlash;
            if (muzzleFlash != null)
            {
                muzzleFlash = Instantiate(gun.muzzleFlash, gun.GetBulletOrigin(_gun.transform)).GetComponent<ParticleSystem>();
                var main = muzzleFlash.main;
                main.playOnAwake = false;
            }
        }
    }

    protected virtual void UseWeapon(Transform weaponScene)
	{
        UseWeapon(weaponScene, weapons[weaponIndex], transform);
    }

    protected virtual void UseWeapon(Transform weaponScene, Weapon weaponAsset, Transform raycastOrigin)
	{
        if (Time.time >= firingEndTime)
		{
            firingEndTime = Time.time + weaponAsset.firingTime;
            
            if (weaponAsset.clip > 0)
		    {
                isFiring = true;
                isReloading = false;

                if (!weaponAsset.isClipInf)
                    weaponAsset.clip--;

                if (weaponAsset.weaponType == WeaponType.HitScan)
                {
                    HitScan(weaponAsset, weaponScene, raycastOrigin);
                }
                else if (weaponAsset.weaponType == WeaponType.Projectile)
                {
                    FireProjectile(weaponAsset, weaponScene);
                }
            }
			else
			{
                isFiring = false;
                isReloading = true;
                weaponAsset.clip = weaponAsset.ammo > weaponAsset.maxClip ? weaponAsset.maxClip : weaponAsset.ammo;
                if (!weaponAsset.isAmmoInf)
				{
                    weaponAsset.ammo -= weaponAsset.maxClip;
                    weaponAsset.ammo = weaponAsset.ammo < 0 ? 0 : weaponAsset.ammo;
				}
			}
		}
		else
		{
            isFiring = false;
            isReloading = false;
		}
	}

    void HitScan(Weapon weaponAsset, Transform weaponScene, Transform raycastOrigin)
    {
        //Transform spawnpoint = weapon.bulletOrigin;
        Ray[] bulletRays = RayDirections(BulletSpread(weaponAsset, raycastOrigin), raycastOrigin);

        if (weaponAsset.bullet != null)
		{
            ParticleSystem bulletParticle = weaponAsset.GetBulletOrigin(weaponScene).Find(weaponAsset.bullet.name + "(Clone)").GetComponent<ParticleSystem>();
            bulletParticle.Play();
		}

        if (weaponAsset.muzzleFlash != null)
		{
            ParticleSystem muzleParticle = weaponAsset.GetBulletOrigin(weaponScene).Find(weaponAsset.muzzleFlash.name + "(Clone)").GetComponent<ParticleSystem>();
            muzleParticle.Play();
		}

        for (int i = 0; i < bulletRays.Length; i++)
        {
            RaycastHit[] hit = Physics.RaycastAll(bulletRays[i], weaponAsset.range);

			for (int n = 0; n < hit.Length; n++)
			{
                if (hit[n].transform != transform && hit[n].transform.GetComponent<Health>())
                {
                    hit[n].transform.GetComponent<Health>().TakeDamage(weaponAsset.damage);
                    break;
                }
			}
        }
    }

    /// <summary>
    /// Refill ammo and clip to the maximum amount
    /// </summary>
    public static void RefillAmmoToMax(Weapon weapon)
	{
        weapon.ammo = weapon.maxAmmo;
        weapon.clip = weapon.maxClip;
	}
    /// <summary>
    /// Refill ammo and clip to a specified amount
    /// </summary>
    /// <param name="refillClip"></param>
    /// <param name="refillAmmo"></param>
    public static void RefillAmmo(Weapon weapon, int refillAmmo)
	{
        weapon.ammo = refillAmmo;
	}


    protected virtual void FireProjectile(Weapon weapon, Transform sceneWeapon)
	{
        Vector2 offset = new Vector2(0, 0);
        Vector3[] bulletSpreadPositions = BulletSpread(weapon, weapon.GetBulletOrigin(sceneWeapon), offset);
        Ray[] bulletRays = RayDirections(bulletSpreadPositions, weapon.GetBulletOrigin(sceneWeapon));

		for (int i = 0; i < bulletRays.Length; i++)
		{
            GameObject bullet = Instantiate(weapon.bullet, weapon.GetBulletOrigin(sceneWeapon));
            bullet.transform.parent = null;
            
            // Get the rigidbody of the projectile, if it has one
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set it's velocity in a specifc dierction with speed
                rb.velocity = bulletRays[i].direction * weapon.shotSpeed;
            }
            
            Projectile projectile = bullet.GetComponent<Projectile>();
            if (projectile != null)
		    {
                projectile.weapon = weapon;
                projectile.layer = gameObject.layer;
                projectile.ownerTag = gameObject.tag;
		    }
			else
			{
                Destroy(bullet, weapon.range);
			}
		}

        if (weapon.muzzleFlash != null)
            weapon.muzzleFlash.Play();
        


    }


    /// <summary>
    /// Returns a set of points local to a transform for raycasting to
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public Vector3[] BulletSpread(Weapon weapon, Transform parent, Vector2 offset = default)
	{
        Vector3[] directions = new Vector3[weapon.bulletCount.x * weapon.bulletCount.y];
		Vector2 half = new Vector2
		{
			x = weapon.bulletCount.x == 1 ? 0 : weapon.bulletCount.x * 0.5f - 0.5f,
			y = weapon.bulletCount.y == 1 ? 0 : weapon.bulletCount.y * 0.5f - 0.5f
		};

        int index = 0;
        for (int y = 0; y < weapon.bulletCount.y; y++)
		{
			for (int x = 0; x < weapon.bulletCount.x; x++)
			{
                // Get point position
                Vector3 position = new Vector3(x - half.x, y - half.y, 0) / weapon.bulletDensity;

                // Convert to transforms rotation
                Quaternion rotation = Quaternion.Euler(parent.eulerAngles);
                Matrix4x4 m = Matrix4x4.Rotate(rotation);
                position = m.MultiplyPoint3x4(position);

                // Apply offset
                position.x += offset.x;
                position.y += offset.y;

                // Set in front of transform at const distence
                position += parent.position + parent.forward * 5;


                directions[index] = position;
                index++;
			}
		}
        return directions;
	}

    public Ray[] RayDirections(Vector3[] destinationPoints, Transform origin)
    {
        Ray[] rays = new Ray[destinationPoints.Length];

		for (int i = 0; i < rays.Length; i++)
		{
            rays[i] = new Ray(origin.position, (destinationPoints[i] - origin.position).normalized);
		}
        return rays;
    }
}
