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

    protected virtual void InstantiateParticles(Transform gunTransform)
	{
		for (int i = 0; i < gunTransform.childCount; i++)
		{
            Weapon item = weapons[i];

            ParticleSystem bullet = item.bullet.GetComponent<ParticleSystem>();
            if (bullet != null)
            {
                
                Instantiate(item.bullet, gunTransform.GetChild(i).Find(item.bulletOrigin.name));
                var main = bullet.main;
                main.playOnAwake = false;
            }

            ParticleSystem muzzleFlash = item.muzzleFlash;
            if (muzzleFlash != null)
            {
                Instantiate(item.muzzleFlash, gunTransform.GetChild(i).Find(item.bulletOrigin.name));
                var main = muzzleFlash.main;
                main.playOnAwake = false;
            }
		}
    }


    protected virtual void UseWeapon(Weapon weapon, Transform raycastOrigin)
	{
        if (Time.time >= firingEndTime)
		{
            firingEndTime = Time.time + weapon.firingTime;
            
            if (weapon.clip > 0)
		    {
                isFiring = true;
                isReloading = false;
                weapon.clip--;

                if(weapon.weaponType == WeaponType.HitScan)
				{
                    HitScan(weapon, raycastOrigin);
				}
                else if (weapon.weaponType == WeaponType.Projectile)
				{
                    FireProjectile(weapon);
				}
		    }
			else
			{
                isFiring = false;
                isReloading = true;
                weapon.clip = weapon.ammo > weapon.maxClip ? weapon.maxClip : weapon.ammo;
                weapon.ammo -= weapon.maxClip;
                weapon.ammo = weapon.ammo < 0 ? 0 : weapon.ammo;
			}
		}
		else
		{
            isFiring = false;
            isReloading = false;
		}
	}

    void HitScan(Weapon weapon, Transform raycastOrigin)
    {
        //Transform spawnpoint = weapon.bulletOrigin;
        Ray[] bulletRays = RayDirections(BulletSpread(weapon, raycastOrigin), raycastOrigin);

        if (weapon.bullet.GetComponent<ParticleSystem>() != null)
            weapon.bullet.GetComponent<ParticleSystem>().Play();

        if (weapon.muzzleFlash != null)
            weapon.muzzleFlash.Play();

        for (int i = 0; i < bulletRays.Length; i++)
        {
            if (Physics.Raycast(bulletRays[i], out RaycastHit hit, weapon.range))
            {
                if (hit.transform.GetComponent<Health>())
                {
                    hit.transform.GetComponent<Health>().TakeDamage(weapon.damage);
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


    protected virtual void FireProjectile(Weapon weapon)
	{
        Ray[] bulletRays = RayDirections(BulletSpread(weapon, weapon.bulletOrigin), weapon.bulletOrigin);

		for (int i = 0; i < bulletRays.Length; i++)
		{
            GameObject bullet = Instantiate(weapon.bullet, weapon.bulletOrigin);
            bullet.transform.parent = null;
            
            // Get the rigidbody of the projectile, if it has one
            Rigidbody rb = weapon.bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Set it's velocity in a specifc dierction with speed
                rb.AddForce(bulletRays[i].direction * weapon.shotSpeed, ForceMode.VelocityChange);
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
