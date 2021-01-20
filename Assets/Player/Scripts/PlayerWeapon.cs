using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : WeaponBehaviour
{
    TextMeshProUGUI ammoText;
    TextMeshProUGUI clipText;

    public Vector2 aimOffset;

    Transform cam;

    public Transform gunViewModel;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
		
        cam = Camera.main.transform;

        ammoText = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("AmmoReserve").GetComponent<TextMeshProUGUI>();
        clipText = FindObjectOfType<GameManager>().PlayerHUD.transform.Find("AmmoClip").GetComponent<TextMeshProUGUI>();

        InstantiateWeapons(gunViewModel);

        weaponIndex = 0;
        CycleWeapons(0, true);
    }

	private void OnDrawGizmosSelected()
	{
        if (weapons.Length > 0)
        {
            Gizmos.color = Color.green;
            Vector3[] points = BulletSpread(weapons[weaponIndex], Camera.main.transform);
            Ray[] rays = RayDirections(points, Camera.main.transform);

            for (int i = 0; i < points.Length; i++)
            {
                Vector3 position = points[i];
                Gizmos.DrawSphere(position, 0.1f);
                Gizmos.DrawRay(rays[i]);
            }
        }
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            CycleWeapons(Mathf.FloorToInt(Input.mouseScrollDelta.y), false);


        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            DisplayAmmo(weapons[weaponIndex].ammo, weapons[weaponIndex].clip);

            if (!weapons[weaponIndex].holdToFire && Input.GetMouseButtonDown(0) || weapons[weaponIndex].holdToFire && Input.GetMouseButton(0))
            {
                UseWeapon(gunViewModel.GetChild(weaponIndex), weapons[weaponIndex], cam);
            }
        }
    }

    void DisplayAmmo(int ammo, int clip)
	{
        if (ammoText != null) ammoText.text = weapons[weaponIndex].isAmmoInf ? "99..." : ammo.ToString();
        if (clipText != null) clipText.text = weapons[weaponIndex].isClipInf ? "99..." : clip.ToString();

    }

    void CycleWeapons(int amount, bool isIndex)
	{
        weaponIndex = isIndex ? amount : weaponIndex + amount;
        weaponIndex = (int)Mathf.Repeat(weaponIndex, weapons.Length);

		// Disable all guns, then enable the selected on.
		for (int i = 0; i < gunViewModel.childCount; i++)
		{
            gunViewModel.GetChild(i).gameObject.SetActive(false);
		}

        gunViewModel.GetChild(weaponIndex).gameObject.SetActive(true);
	}
}
