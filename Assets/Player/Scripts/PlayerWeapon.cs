using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public int activeWeapon;

    public Text ammoText;
    public Text clipText;

    public Vector2 aimOffset;

    public WeaponBehaviour[] weapons;

    Transform cam;

    GameObject gunViewModel;

    // Start is called before the first frame update
    void Start()
    {
		foreach (var weapon in weapons)
		{
		    weapon.RefillAmmoToMax();
		}
		cam = Camera.main.transform;

        gunViewModel = transform.GetChild(1).GetChild(2).GetChild(1).gameObject;

        activeWeapon = 0;
        CycleWeapons(0, true);
    }

	private void OnDrawGizmosSelected()
	{
        if (weapons.Length > 0)
        {
            Gizmos.color = Color.green;
            Vector3[] points = weapons[activeWeapon].BulletSpread(aimOffset, Camera.main.transform);
            Ray[] rays = weapons[activeWeapon].RayDirections(points, Camera.main.transform);

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 position = points[i];
                Gizmos.DrawSphere(position, 0.1f);
                Gizmos.DrawRay(rays[i]);
            }
        }
    }

	float endTime;
    void Update()
    {
        CycleWeapons(Mathf.FloorToInt(Input.mouseScrollDelta.y), false);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CycleWeapons(0, true);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
            CycleWeapons(1, true);
		}


        if (activeWeapon >= 0 && activeWeapon < weapons.Length)
        {
            DisplayAmmo(weapons[activeWeapon].Ammo, weapons[activeWeapon].Clip);
            // Fire is cooldown has finished
            if (Time.time >= endTime)
            {
                if (Input.GetMouseButton(0))
                {
                    endTime = weapons[activeWeapon].fireingTime + Time.time;


                    // Check that there's enough ammo
                    if (weapons[activeWeapon].Clip > 0)
                    {
                        weapons[activeWeapon].Clip--;

                        if (weapons[activeWeapon].weaponType == WeaponType.HitScan)
                        {
                            HitScan();
                        }
                        else if (weapons[activeWeapon].weaponType == WeaponType.Projectile)
                        {
                            FireProjectile(weapons[activeWeapon]);
                        }

                    }
                    else
                    {
                        // check if there's anough ammo left in reserve
                        // to refill clip all the way
                        if (weapons[activeWeapon].Ammo == 0)
                        {
                            return;
                        }
                        // only refill clip to what is left in reserve
                        else if (weapons[activeWeapon].Ammo < weapons[activeWeapon].maxClipSize)
                        {
                            // Play reload animation here:

                            // Whatever ammo is left is put into the clip
                            weapons[activeWeapon].Clip = weapons[activeWeapon].Ammo;

                            // the reserve ammo is now empty
                            weapons[activeWeapon].Ammo = 0;
                        }
                        // If there's enough left in reserve to completly fill the clip
                        else
                        {
                            // Play reload animation here:

                            weapons[activeWeapon].Clip = weapons[activeWeapon].maxClipSize;
                            weapons[activeWeapon].Ammo -= weapons[activeWeapon].maxClipSize;

                        }
                    }
                }
            }
        }
    }

    void FireProjectile(WeaponBehaviour weapon)
	{
        if (weapon.projectile == null)
        {
            Debug.LogError(weapon.name + " doesn't have a gun model");
            return;
        }
        // Spawn the projectile
        GameObject projectile = Instantiate(weapon.projectile, weapon.ProjectileSpawnPoint.position, new Quaternion());

        projectile.GetComponent<ProjectileDeathTimer>().ownerTag = "Player";

        // Fire the projectile
        weapon.FireProjectile(projectile, cam.forward);
    }

    void HitScan()
	{
        Transform spawnpoint = weapons[activeWeapon].ProjectileSpawnPoint;
        Ray[] rays = weapons[activeWeapon].RayDirections(weapons[activeWeapon].BulletSpread(aimOffset, spawnpoint), spawnpoint);

		for (int i = 0; i < rays.Length; i++)
		{
            GameObject trail = Instantiate(weapons[activeWeapon].bulletTrail, spawnpoint.position, new Quaternion());
            trail.transform.rotation = Quaternion.LookRotation(rays[i].direction);
            trail.GetComponent<BulletTrail>().lifeTime = weapons[activeWeapon].range;
            

			if (Physics.Raycast(rays[i], out RaycastHit hit, weapons[activeWeapon].range))
			{
                trail.GetComponent<BulletTrail>().lifeTime = hit.distance;

				if (hit.transform.GetComponent<Health>())
				{
					hit.transform.GetComponent<Health>().TakeDamage(weapons[activeWeapon].damage);
				}
			}
		}

	}

    void DisplayAmmo(int ammo, int clip)
	{
        if (ammoText != null) ammoText.text = ammo.ToString();
        if (clipText != null) clipText.text = clip.ToString();
	}

    void CycleWeapons(int amount, bool isIndex)
	{
        activeWeapon = isIndex ? amount : activeWeapon + amount;
        activeWeapon = (int)Mathf.Repeat(activeWeapon, weapons.Length);

        if (gunViewModel.transform.childCount > 0) Destroy(gunViewModel.transform.GetChild(0).gameObject);
        Instantiate(weapons[activeWeapon].gunModel, gunViewModel.transform);
	}
}
