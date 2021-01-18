using UnityEngine;


public class WeaponBehaviour : MonoBehaviour
{




    /// <summary>
    /// Refill ammo and clip to the maximum amount
    /// </summary>
    public void RefillAmmoToMax(Weapon weapon)
	{
        weapon.ammo = weapon.maxAmmo;
        weapon.clip = weapon.maxClip;
	}
    /// <summary>
    /// Refill ammo and clip to a specified amount
    /// </summary>
    /// <param name="refillClip"></param>
    /// <param name="refillAmmo"></param>
    public void RefillAmmo(Weapon weapon, int refillAmmo)
	{
        weapon.ammo = refillAmmo;
	}


    public void FireProjectile(Weapon weapon, Vector3 fireDirection)
	{

        // Get the rigidbody of the projectile, if it has one
        Rigidbody rb = weapon.bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            
            // Set it's velocity in a specifc dierction with speed
            rb.AddForce(fireDirection * weapon.shotSpeed, ForceMode.VelocityChange);
        }

        ProjectileDeathTimer deathTimer = weapon.bullet.GetComponent<ProjectileDeathTimer>();
        if (deathTimer != null)
		{
            deathTimer.lifeTime = weapon.range;
		}
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
