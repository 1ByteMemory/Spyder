using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
	LevelSelection selection;
	GameManager gm;
	GameObject player;
	PlayerController pc;
	PlayerWeapon playerWeapons;
	SavesContainer savesContainer = new SavesContainer();

	string sceneName;
	string date;
	string time;

	// Start is called before the first frame update
	void Start()
    {
		selection = GetComponentInChildren<LevelSelection>();
		gm = GetComponent<GameManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		pc = player.GetComponent<PlayerController>();
		playerWeapons = player.GetComponent<PlayerWeapon>();
		sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
     
		if (Input.GetKeyDown(KeyCode.F5))
		{
			// Quick Save
			SaveInfo lvl = new SaveInfo();
			lvl.abilityUnlocked = pc.isAbilityUnlocked;
			lvl.sceneName = sceneName; // null
			lvl.spawnPoint = player.transform.position;
			
			date = System.DateTime.Now.ToShortDateString().ToString(new CultureInfo("en-US")).Replace("/", "-");
			time = System.DateTime.Now.ToShortTimeString().Replace(":", "");

			lvl.title = date + " " + time;

			lvl.availableWeapons = new Gun[playerWeapons.weapons.Count];
			for (int i = 0; i < playerWeapons.weapons.Count; i++)
			{
				Weapon wep = playerWeapons.weapons[i];
				lvl.availableWeapons[i] = new Gun(wep.Name, wep.ammo, wep.clip);
			}
			lvl.foundKeys = KeycardIcon.keysFound;
			lvl.dimension = (int)GameManager.currentActiveDimension;

			Debug.Log("Saving");
			savesContainer.SaveToXml(Path.Combine(Application.persistentDataPath, + lvl.title + ".xml"), lvl);
		}

		if (Input.GetKeyDown(KeyCode.F6))
		{
			// Quick Load


		}
    }
}
