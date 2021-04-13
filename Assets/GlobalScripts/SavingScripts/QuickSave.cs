using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;

public class QuickSave : MonoBehaviour
{

	private static GameObject player;
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
			Save("quicksaves");
		}

		if (Input.GetKeyDown(KeyCode.F6))
		{
			// Quick Load
			Debug.Log("Loading...");

			mostRecentLoad = SavesContainer.LoadFromXml(Path.Combine(Application.persistentDataPath, "_Recent" + ".xml"));
			GameManager.loadedFromSave = true;

			SceneLoader.Load_Scene(mostRecentLoad.sceneName);
		}

		if (Input.GetKeyDown(KeyCode.F9))
		{
			Debug.Log("Saving a showcase load. File can be found here:\n<color=blue>" + Application.persistentDataPath + "/showcase</color>");
			Save("showcase");
		}

	}



	public static void Save(string destinationFolder)
	{
		date = System.DateTime.Now.ToShortDateString().ToString(new CultureInfo("en-US")).Replace("/", "-");
		time = System.DateTime.Now.ToShortTimeString().Replace(":", "");

		Save(destinationFolder, date + " " + time);
	}

	public static void Save(string destinationFolder, string fileName)
	{
		FindObjectOfType<GameManager>().GetComponentInChildren<Animator>().SetTrigger("Play");

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
		lvl.foundKeys = KeycardIcon.keysFound;
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
}
