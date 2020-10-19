using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public Text ammoText;
    public Text clipText;


    public WeaponBehaviour weapon;

    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
		weapon.RefillAmmoToMax();
        cam = Camera.main.transform;
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
                endTime = weapon.fireingSpeed + Time.time;


                // Check that there's enough ammo
                if (weapon.Clip > 0)
                {
                    weapon.Clip--;

                    // Spawn the projectile
                    GameObject projectile = Instantiate(weapon.projectile, weapon.ProjectileSpawnPoint.position, new Quaternion());

                    // Fire the projectile
                    weapon.FireProjectile(projectile, cam.forward);
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


    void DisplayAmmo(int ammo, int clip)
	{
        if (ammoText != null) ammoText.text = ammo.ToString();
        if (clipText != null) clipText.text = clip.ToString();
	}

}
