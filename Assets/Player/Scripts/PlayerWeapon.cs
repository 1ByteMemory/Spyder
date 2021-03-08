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

    private PlayerMovement pm;

    public float scrollSensitivity = 1;

    public LayerMask layer_Mask;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
		
        cam = Camera.main.transform;

        pm = GetComponent<PlayerMovement>();

        GameManager gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        ammoText = gm.PlayerHUD.transform.Find("AmmoReserve").GetComponent<TextMeshProUGUI>();
        clipText = gm.PlayerHUD.transform.Find("AmmoClip").GetComponent<TextMeshProUGUI>();


        InstantiateWeapons(gunViewModel);

        weaponIndex = 0;
        CycleWeapons(0, true);
    }

	private void OnDrawGizmosSelected()
	{
        if (weapons.Count > 0)
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
        if (!GameManager.IsPaused)
        {
            //float scrollValue = Input.mouseScrollDelta.y * scrollSensitivity;
            if (Input.mouseScrollDelta.y != 0)
            {
                float num = Input.mouseScrollDelta.y;
                num = num > 0 ? -1 : 1;

                CycleWeapons((int)num, false);
            }

            if (gunViewModel.childCount > 0)
            {

                Transform activeGun = gunViewModel.GetChild(weaponIndex);

                if (weaponIndex >= 0 && weaponIndex < weapons.Count)
                {
                    DisplayAmmo(weapons[weaponIndex].ammo, weapons[weaponIndex].clip);

                    if (!weapons[weaponIndex].holdToFire && Input.GetMouseButtonDown(0) || weapons[weaponIndex].holdToFire && Input.GetMouseButton(0))
                    {
                        UseWeapon(activeGun, weapons[weaponIndex], cam);
                    }
                }

                Animator anim = activeGun.GetComponentInChildren<Animator>();
                if (pm.IsMoving())
                {
                    if (anim != null)
                    {
                        anim.SetBool("Is Moving", true);
                    }
                }
                else
                {
                    if (anim != null)
                    {
                        anim.SetBool("Is Moving", false);
                    }
                }
            }
        }
    }

    void DisplayAmmo(int ammo, int clip)
	{
        if (ammoText != null) ammoText.text = weapons[weaponIndex].isAmmoInf ? "99..." : ammo.ToString();
        if (clipText != null) clipText.text = weapons[weaponIndex].isClipInf ? "99..." : clip.ToString();

    }

    public void CycleWeapons(int amount, bool isIndex)
	{
        weaponIndex = isIndex ? amount : weaponIndex + amount;
        weaponIndex = (int)Mathf.Repeat(weaponIndex, weapons.Count);

		// Disable all guns, then enable the selected on.
		for (int i = 0; i < gunViewModel.childCount; i++)
		{
            gunViewModel.GetChild(i).gameObject.SetActive(false);
		}

        if (gunViewModel.childCount > 0)
            gunViewModel.GetChild(weaponIndex).gameObject.SetActive(true);
	}
}
