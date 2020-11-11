using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponBehaviour
{
    public bool isHitscan;
    public string name;
    public GameObject projectile;
    public Transform ProjectileSpawnPoint;

    [Header("")]

    [Min(1)]
    public int bulletSpreadX = 1;
    [Min(1)]
    public int bulletSpreadY = 1;
    public float bulletsDensity;

    /// <summary>
    /// How fast it takes to fire the next shot
    /// </summary>
    [Tooltip("How fast it takes to fire the next shot")]
    public float fireingTime;

    [Header("Projectile")]
    /// <summary>
    /// How fast the projectile fires
    /// </summary>
    [Tooltip("How fast the projectile fires")]
    public float projectileShotSpeed;
    //public float reloadSpeed;
    public float projectileLifeTime;
    
    [Header("Ammo")]
    public int maxAmmo;
    public int maxClipSize;

    private int _ammo;
    private int _clip;
	private object rays;

	public int Ammo
	{
        get { return _ammo; }
        set	{ _ammo = value >= 0 ? value : 0; }
	}

    public int Clip
	{
        get { return _clip; }
        set { _clip = value >= 0 ? value : 0; }

    }


    /// <summary>
    /// Refill ammo and clip to the maximum amount
    /// </summary>
    public void RefillAmmoToMax()
	{
        Ammo = maxAmmo;
        Clip = maxClipSize;
	}
    /// <summary>
    /// Refill ammo and clip to a specified amount
    /// </summary>
    /// <param name="refillClip"></param>
    /// <param name="refillAmmo"></param>
    public void RefillAmmo(int refillClip, int refillAmmo)
	{
        Ammo = refillAmmo;
        Clip = refillClip;
	}

    public static void FireProjectile(GameObject projectile, Vector3 fireDirection, float shotSpeed, float projectileLifeTime)
	{
        // Get the rigidbody of the projectile, if it has one
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
		{
            // Set it's velocity in a specifc dierction with speed
            rb.AddForce(fireDirection * shotSpeed, ForceMode.VelocityChange);
		}

		Object.Destroy(projectile, projectileLifeTime);
	}
    public void FireProjectile(GameObject gameObject, Vector3 fireDirection)
	{

        // Get the rigidbody of the projectile, if it has one
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            
            // Set it's velocity in a specifc dierction with speed
            rb.AddForce(fireDirection * projectileShotSpeed, ForceMode.VelocityChange);
        }

        ProjectileDeathTimer deathTimer = projectile.GetComponent<ProjectileDeathTimer>();
        if (deathTimer != null)
		{
            deathTimer.lifeTime = projectileLifeTime;
		}
    }


    public Vector3[] BulletSpread(Vector2 offset)
	{
        Vector3[] directions = new Vector3[bulletSpreadX * bulletSpreadY];
        Vector2 half = new Vector2();

        half.x = bulletSpreadX == 1 ? 0 : bulletSpreadX * 0.5f - 0.5f;
        half.y = bulletSpreadY == 1 ? 0 : bulletSpreadY * 0.5f - 0.5f;


        Debug.Log(half);
        int index = 0;
        for (int y = 0; y < bulletSpreadY; y++)
		{
			for (int x = 0; x < bulletSpreadX; x++)
			{
                Vector3 position = new Vector3(x - half.x, y - half.y, 0) / bulletsDensity;
                position.x += offset.x;
                position.y += offset.y;

                directions[index] = position;
                index++;
			}
		}
        return directions;
	}
}
