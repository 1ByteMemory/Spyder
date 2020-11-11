using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public Text ammoText;
    public Text clipText;

    public Vector2 aimOffset;

    public WeaponBehaviour weapon;

    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
		weapon.RefillAmmoToMax();
        cam = Camera.main.transform;
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.green;
        Vector3[] points = weapon.BulletSpread(aimOffset, Camera.main.transform);
        Ray[] rays = weapon.RayDirections(points, Camera.main.transform);

		for (int i = 0; i < points.Length; i++)
		{
            Vector3 position = points[i];
            Gizmos.DrawSphere(position, 0.1f);
            Gizmos.DrawRay(rays[i]);
		}
    }

	float endTime;
    void Update()
    {
        DisplayAmmo(weapon.Ammo, weapon.Clip);

        // Fire is cooldown has finished
        if (Time.time >= endTime)
        {
            if (Input.GetMouseButton(0))
            {
                endTime = weapon.fireingTime + Time.time;


                // Check that there's enough ammo
                if (weapon.Clip > 0)
                {
                    weapon.Clip--;

                    if (weapon.isHitscan)
					{
                        HitScan();
					}
					else
					{
                        FireProjectile(weapon);
					}

                }
                else
                {
                    // check if there's anough ammo left in reserve
                    // to refill clip all the way
                    if (weapon.Ammo == 0)
                    {
                        return;
                    }
                    // only refill clip to what is left in reserve
                    else if (weapon.Ammo < weapon.maxClipSize)
                    {
                        // Play reload animation here:

                        // Whatever ammo is left is put into the clip
                        weapon.Clip = weapon.Ammo;

                        // the reserve ammo is now empty
                        weapon.Ammo = 0;
                    }
                    // If there's enough left in reserve to completly fill the clip
                    else
                    {
                        // Play reload animation here:

                        weapon.Clip = weapon.maxClipSize;
                        weapon.Ammo -= weapon.maxClipSize;

                    }
                }
            }
        }
    }

    void FireProjectile(WeaponBehaviour weapon)
	{
        // Spawn the projectile
        GameObject projectile = Instantiate(weapon.projectile, weapon.ProjectileSpawnPoint.position, new Quaternion());

        projectile.GetComponent<ProjectileDeathTimer>().ownerTag = "Player";

        // Fire the projectile
        weapon.FireProjectile(projectile, cam.forward);
    }

    void HitScan()
	{
        Transform spawnpoint = weapon.ProjectileSpawnPoint;
        Ray[] rays = weapon.RayDirections(weapon.BulletSpread(aimOffset, spawnpoint), spawnpoint);

		for (int i = 0; i < rays.Length; i++)
		{
            GameObject trail = Instantiate(weapon.bulletTrail, spawnpoint.position, new Quaternion());
            trail.transform.rotation = Quaternion.LookRotation(rays[i].direction);
            trail.GetComponent<BulletTrail>().lifeTime = weapon.range;
            

			if (Physics.Raycast(rays[i], out RaycastHit hit, weapon.range))
			{
                trail.GetComponent<BulletTrail>().lifeTime = hit.distance;

				if (hit.transform.GetComponent<Health>())
				{
					hit.transform.GetComponent<Health>().TakeDamage(weapon.damage);
				}
			}
		}

	}

    void DisplayAmmo(int ammo, int clip)
	{
        if (ammoText != null) ammoText.text = ammo.ToString();
        if (clipText != null) clipText.text = clip.ToString();
	}
}
