﻿using System.Collections;
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

	public static SaveInfo mostRecentSave;

	public Weapon[] weapons;

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
			SaveInfo lvl = new SaveInfo
			{
				abilityUnlocked = pc.isAbilityUnlocked,
				sceneName = sceneName,
				spawnPoint = player.transform.position,
				spawnRotation = player.transform.eulerAngles
			};

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
			lvl.dimension = (int)GameManager.currentDimension;

			lvl.health = player.GetComponent<PlayerHealth>().currentHealth;

			Debug.Log("Saving");

			// Save with the time and date in the saves folder
			string savesFolder = Path.Combine(Application.persistentDataPath, "saves");


			if (!File.Exists(savesFolder))
			{
				Directory.CreateDirectory(savesFolder);
			}

			savesContainer.SaveToXml(Path.Combine(savesFolder, lvl.title + ".xml"), lvl);
			
			// Save as the most recent
			savesContainer.SaveToXml(Path.Combine(Application.persistentDataPath, "_Recent" + ".xml"), lvl);
		}

		if (Input.GetKeyDown(KeyCode.F6))
		{
			// Quick Load
			Debug.Log("Loading...");

			mostRecentSave = SavesContainer.LoadFromXml(Path.Combine(Application.persistentDataPath, "_Recent" + ".xml"));
			GameManager.loadedFromSave = true;

			SceneLoader.Load_Scene(mostRecentSave.sceneName);

		}
    }
}
