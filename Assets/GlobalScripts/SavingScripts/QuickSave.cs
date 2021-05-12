using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;

public class QuickSave : MonoBehaviour
{

	private static GameObject player;
	private static GameManager gm;
	private static PlayerController pc;
	private static PlayerWeapon playerWeapons;
	private static SavesContainer savesContainer = new SavesContainer();

	private static string sceneName;
	private static string date;
	private static string time;

	public static SaveInfo mostRecentLoad;

	public Weapon[] weapons;

	// Start is called before the first frame update
	void Start()
    {
		gm = FindObjectOfType<GameManager>();
		player = GameObject.FindGameObjectWithTag("Player");
		if (player != null)
		{
			pc = player.GetComponent<PlayerController>();
			playerWeapons = player.GetComponent<PlayerWeapon>();
		}
		sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    
	}

	public void QuickLoad()
	{
		Debug.Log("Loading...");

		mostRecentLoad = SavesContainer.LoadFromXml(Path.Combine(Application.persistentDataPath, "_Recent" + ".xml"));
		GameManager.loadedFromSave = true;

		SceneLoader.Load_Scene(mostRecentLoad.sceneName);
	}

	public void ClickSave(string destinationFolder)
	{
		Save(destinationFolder);
	}

	public static void Save(string destinationFolder)
	{
		date = System.DateTime.Now.ToShortDateString().ToString(new CultureInfo("en-US")).Replace("/", "-");
		time = System.DateTime.Now.ToShortTimeString().Replace(":", "");

		Save(destinationFolder, date + " " + time);
	}

	public static void Save(string destinationFolder, string fileName)
	{
		gm.PlayerHUD.GetComponentInChildren<Animator>().SetTrigger("Play");

		SaveInfo lvl = new SaveInfo
		{
			abilityUnlocked = pc.isAbilityUnlocked,
			sceneName = sceneName,
			spawnPoint = player.transform.position,
			spawnRotation = player.transform.eulerAngles
		};

		lvl.title = fileName;

		lvl.availableWeapons = new Gun[playerWeapons.weapons.Count];
		for (int i = 0; i < playerWeapons.weapons.Count; i++)
		{
			Weapon wep = playerWeapons.weapons[i];
			lvl.availableWeapons[i] = new Gun(wep.Name, wep.ammo, wep.clip);
		}
		lvl.foundKeys = PlayerHUD.keysFound;

		lvl.enemiesKilled = gm.killedEnemiesID.ToArray();
		lvl.dimension = (int)GameManager.currentDimension;

		lvl.health = player.GetComponent<PlayerHealth>().currentHealth;

		Debug.Log("Saving");

		// Save with the time and date in the saves folder
		string savesFolder = Path.Combine(Application.persistentDataPath, destinationFolder);


		if (!File.Exists(savesFolder))
		{
			Directory.CreateDirectory(savesFolder);
		}

		savesContainer.SaveToXml(Path.Combine(savesFolder, lvl.title + ".xml"), lvl);

		// Save as the most recent
		savesContainer.SaveToXml(Path.Combine(Application.persistentDataPath, "_Recent" + ".xml"), lvl);
	}


	public void SaveShowcase()
	{
		FindObjectOfType<GameManager>().GetComponentInChildren<Animator>().SetTrigger("Play");

		SaveInfo lvl = new SaveInfo
		{
			abilityUnlocked = pc.isAbilityUnlocked,
			sceneName = sceneName,
			spawnPoint = player.transform.position,
			spawnRotation = player.transform.eulerAngles
		};

		lvl.title = "TITLE " + System.DateTime.Now.ToShortTimeString().Replace(":", "");

		lvl.availableWeapons = new Gun[playerWeapons.weapons.Count];
		for (int i = 0; i < playerWeapons.weapons.Count; i++)
		{
			Weapon wep = playerWeapons.weapons[i];
			lvl.availableWeapons[i] = new Gun(wep.Name, wep.ammo, wep.clip);
		}
		lvl.foundKeys = PlayerHUD.keysFound;
		lvl.enemiesKilled = gm.killedEnemiesID.ToArray();
		lvl.dimension = (int)GameManager.currentDimension;

		lvl.health = player.GetComponent<PlayerHealth>().currentHealth;

		Debug.Log("Saving");

		string savesFolder = Application.dataPath + "/Resources/Showcase Saves";

		savesContainer.SaveToXml(Path.Combine(savesFolder, lvl.title + ".xml"), lvl);
	}
}
