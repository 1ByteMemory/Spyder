using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : WeaponBehaviour
{
    public int activeWeapon;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;

    public Vector2 aimOffset;

    Weapon[] weapons;

    Transform cam;

    public GameObject gunViewModel;

    // Start is called before the first frame update
    void Start()
    {
		cam = Camera.main.transform;

        ammoText = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("AmmoReserve").GetComponent<TextMeshProUGUI>();
        clipText = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("AmmoClip").GetComponent<TextMeshProUGUI>();
        

        activeWeapon = 0;
        CycleWeapons(0, true);
    }

	private void OnDrawGizmosSelected()
	{
        if (weapons.Length > 0)
        {
            Gizmos.color = Color.green;
            Vector3[] points = BulletSpread(weapons[activeWeapon], Camera.main.transform);
            Ray[] rays = RayDirections(points, Camera.main.transform);

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
            DisplayAmmo(weapons[activeWeapon].ammo, weapons[activeWeapon].clip);
            // Fire is cooldown has finished
            if (Time.time >= endTime)
            {
                if (Input.GetMouseButton(0))
                {
                    endTime = weapons[activeWeapon].firingTime + Time.time;

                    // Check that there's enough ammo
                    if (weaponsBehavoirs[activeWeapon].Clip > 0)
                    {
                        weaponsBehavoirs[activeWeapon].Clip--;

                        if (weaponsBehavoirs[activeWeapon].weaponType == WeaponType.HitScan)
                        {
                            HitScan();
                        }
                        else if (weaponsBehavoirs[activeWeapon].weaponType == WeaponType.Projectile)
                        {
                            FireProjectile(weaponsBehavoirs[activeWeapon]);
                        }

                    }
                    else
                    {
                        // check if there's anough ammo left in reserve
                        // to refill clip all the way
                        if (weaponsBehavoirs[activeWeapon].Ammo == 0)
                        {
                            return;
                        }
                        // only refill clip to what is left in reserve
                        else if (weaponsBehavoirs[activeWeapon].Ammo < weaponsBehavoirs[activeWeapon].maxClipSize)
                        {
                            // Play reload animation here:

                            // Whatever ammo is left is put into the clip
                            weaponsBehavoirs[activeWeapon].Clip = weaponsBehavoirs[activeWeapon].Ammo;

                            // the reserve ammo is now empty
                            weaponsBehavoirs[activeWeapon].Ammo = 0;
                        }
                        // If there's enough left in reserve to completly fill the clip
                        else
                        {
                            // Play reload animation here:

                            weaponsBehavoirs[activeWeapon].Clip = weaponsBehavoirs[activeWeapon].maxClipSize;
                            weaponsBehavoirs[activeWeapon].Ammo -= weaponsBehavoirs[activeWeapon].maxClipSize;

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
        projectile.GetComponent<ProjectileDeathTimer>().normalDamage = weapon.damage;

        // Fire the projectile
        weapon.FireProjectile(projectile, cam.forward);
    }

    void HitScan()
	{
        Transform spawnpoint = weaponsBehavoirs[activeWeapon].ProjectileSpawnPoint;
        Ray[] rays = weaponsBehavoirs[activeWeapon].RayDirections(weaponsBehavoirs[activeWeapon].BulletSpread(aimOffset, spawnpoint), spawnpoint);

		for (int i = 0; i < rays.Length; i++)
		{
            GameObject trail = Instantiate(weaponsBehavoirs[activeWeapon].bulletTrail, spawnpoint.position, new Quaternion());
            trail.transform.rotation = Quaternion.LookRotation(rays[i].direction);
            trail.GetComponent<BulletTrail>().lifeTime = weaponsBehavoirs[activeWeapon].range;
            

			if (Physics.Raycast(rays[i], out RaycastHit hit, weaponsBehavoirs[activeWeapon].range))
			{
                trail.GetComponent<BulletTrail>().lifeTime = hit.distance;

				if (hit.transform.GetComponent<Health>())
				{
					hit.transform.GetComponent<Health>().TakeDamage(weaponsBehavoirs[activeWeapon].damage);
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
        activeWeapon = (int)Mathf.Repeat(activeWeapon, weaponsBehavoirs.Length);

        if (gunViewModel.transform.childCount > 0) Destroy(gunViewModel.transform.GetChild(0).gameObject);
        Instantiate(weaponsBehavoirs[activeWeapon].gunModel, gunViewModel.transform);
	}
}
