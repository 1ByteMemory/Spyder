using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
//using Fungus;

public enum Dimension
{
	Digital,
	Real
}

public class GameManager : MonoBehaviour
{
	public const string version = "a0.26.3";

	[Header("Scene Loader")]
	//public bool addScene;
	bool isLoaded;
	//public string sceneToLoad;
	public bool loadEnemies = true;

	[Header("UI and HUDs")]
	public GameObject PauseMenu;
	public GameObject SettingsUI;
	public GameObject PlayerHUD;
	public GameObject DeathMenuUI;
	//public GameObject Fungus;

	public static Dimension currentDimension;
	public static int activeLayer;

	[Header("Dimension Switching")]
	public AudioClip dimensionClip;
	private AudioSource src;

	public GameObject realWorldObjects;
	public GameObject digitalWorldObjects;

	public PostProcessProfile retroProfile;

	[Header("")]
	public bool spawnAtSpawnPoint = true;
	public Transform spawnPoint;

	Camera mainCam;

	ReplacmentShader digitalCam;
	ReplacmentShader realCam;

	static PlayerMovement playerMove;

	public static AudioClip barkToPlay;
	
	[HideInInspector]
	public List<GameObject> seenEnemies = new List<GameObject>();
	//private Flowchart flowchart;

	public Material[] digitalMats;

	static bool pauseToggel;
	public static bool IsGameWin;

	public Key[] keys;

	// When loaded from level selector
	public static Vector3 loadedSpawnPosition;
	public static Weapon[] loadedWeapons;
	public static bool loadedFromSelector;
	public static bool loadedFromSave;
	public void LoadedFromMainMenu()
	{
		// When the player selects new game
		loadedFromSave = false;
		loadedFromSelector = false;
	}

	private QuickSave quickSave;

	public readonly List<int> killedEnemiesID = new List<int>();
	public bool HasEnemyID(int id)
	{
		foreach (int idItem in killedEnemiesID)
		{
			if (idItem == id)
			{
				return true;
			}
		}
		return false;
	}
	public void AddKilledEnemy(int id)
	{
		if (!HasEnemyID(id))
		{
			killedEnemiesID.Add(id);
		}
	}

	private void Start()
	{
		playerMove = FindObjectOfType<PlayerMovement>();

		if (playerMove != null)
		{
			// reset pause toggle
			pauseToggel = false;

			SetMouseActive(false);

			quickSave = GetComponent<QuickSave>();

			mainCam = Camera.main;

			realCam = mainCam.transform.Find("RealWorldCam").gameObject.GetComponent<ReplacmentShader>();
			digitalCam = mainCam.transform.Find("DigitalWorldCam").gameObject.GetComponent<ReplacmentShader>();



			Time.timeScale = 1;

			if (realWorldObjects != null)
			{
				// Asign objects to layers
				for (int i = 0; i < realWorldObjects.transform.childCount; i++)
				{
					realWorldObjects.transform.GetChild(i).gameObject.layer = 9; // the real world layer
				}
			}


			if (digitalWorldObjects != null)
			{
				for (int i = 0; i < digitalWorldObjects.transform.childCount; i++)
				{
					digitalWorldObjects.transform.GetChild(i).gameObject.layer = 8; // the digital layer
				}
			}

			src = GetComponent<AudioSource>();

			SettingsUI = UI(SettingsUI);
			PauseMenu = UI(PauseMenu);
			PlayerHUD = UI(PlayerHUD);
			DeathMenuUI = UI(DeathMenuUI);
			//Fungus = UI(Fungus);


			//flowchart = Fungus.GetComponentInChildren<Flowchart>();
			//StartCoroutine(StartBarks());


			SettingsUI.SetActive(false);
			PauseMenu.SetActive(false);
			DeathMenuUI.SetActive(false);

			if (!loadEnemies)
			{
				Debug.LogWarning("All enemies have been disabled. To enable them, go to _GameManager and set Load Enemies to true.");
				GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
				foreach (GameObject enemy in enemies)
				{
					enemy.SetActive(false);
				}
				EnemyWaveSpawner[] spawners = GameObject.FindObjectsOfType<EnemyWaveSpawner>();
				foreach (EnemyWaveSpawner spawner in spawners)
				{
					spawner.gameObject.SetActive(false);
				}
			}

			// Set the active dimension to the real
			SetDimension(Dimension.Real);

			// Reset scan distance to zero
			ScannerEffect.ScanDistance = 0;

			// Reset keys found
			global::PlayerHUD.keysFound = null;

			PlayerWeapon pw = playerMove.GetComponent<PlayerWeapon>();
			if (loadedFromSelector)
			{
				loadedFromSelector = false;

				if (loadedSpawnPosition != Vector3.zero)
					GoToSpawn(loadedSpawnPosition);


				pw.weapons.Clear();
				for (int i = 0; i < loadedWeapons.Length; i++)
				{
					pw.weapons.Add(loadedWeapons[i]);
				}


			}
			else if (loadedFromSave)
			{
				loadedFromSave = false;

				// ----- Weapons ----- //

				pw.weapons.Clear();
				Gun[] savedWeapons = QuickSave.mostRecentLoad.availableWeapons;
				for (int i = 0; i < savedWeapons.Length; i++)
				{
					// Get weapon from save
					//QuickSave quicksave = GetComponent<QuickSave>();
					Weapon weapon = WeaponBehaviour.GetWeapon(quickSave.weapons, savedWeapons[i].name);

					// Set ammo and clip from save

					// add weapon to player
					pw.weapons.Add(weapon);

					// Ammo set in PlayerWeapon
					PlayerWeapon.loadedFromSave = true;
				}

				// ----- Position and Rotation ----- //
				GoToSpawn(QuickSave.mostRecentLoad.spawnPoint, QuickSave.mostRecentLoad.spawnRotation);

				// ----- Health ----- //
				// Health is set on the PlayerHealth script
				PlayerHealth.loadedFromSave = true;


				// ----- Dimension ----- //
				Dimension savedDim = (Dimension)QuickSave.mostRecentLoad.dimension;
				if (savedDim == Dimension.Real)
				{
					// Set dimension to digital first so it doesn't mess up some stuff.
					SetDimension(Dimension.Digital);
				}
				SetDimension(savedDim);

				ScannerEffect.ScanDistance = currentDimension == (Dimension)0 ? 200 : 0.1f;

				// ----- Ability ----- //
				playerMove.GetComponent<PlayerController>().isAbilityUnlocked = QuickSave.mostRecentLoad.abilityUnlocked;

				// ----- Keys Found ----- //
				global::PlayerHUD.keysFound = QuickSave.mostRecentLoad.foundKeys;

				// ----- Enemies Killed ----//
				int[] enemies = QuickSave.mostRecentLoad.enemiesKilled;
				killedEnemiesID.Clear();
				foreach (int item in enemies)
				{
					killedEnemiesID.Add(item);

					// Killed Enemies are destroied on the SearchAndDestroy script
				}
			}
			else if (spawnAtSpawnPoint) GoToSpawn();



			// Add Death Menu to OnDeath
			DeathEffectController deathEffect = GetComponentInChildren<DeathEffectController>();
			deathEffect.PlayerDeath += DeathMenu;
		}
		ApplySettings();
	}


	#region Enemy Barks
	public void AddEnemy(GameObject enemy)
	{
		seenEnemies.Add(enemy);
	}
	public void RemoveEnemy(GameObject enemy)
	{
		seenEnemies.Remove(enemy);
	}

	public void SetAudioBark(AudioClip clip)
	{
		Debug.Log("Setting Bark");
		barkToPlay = clip;
	}


	private float barkVolume;
	public void PlayBark()
	{
		List<GameObject> _seenEnemies = FindObjectOfType<GameManager>().seenEnemies;
		if (_seenEnemies.Count > 0)
		{
			// Selects random enemy from list
			GameObject enemy = _seenEnemies[Random.Range(0, _seenEnemies.Count)];

			AudioSource src = enemy.GetComponent<AudioSource>();
			// if Enemy has audio compnatn
			if (src != null && barkToPlay != null)
			{
				Debug.Log("playing " + barkToPlay + " on " + src.gameObject);
				src.clip = barkToPlay;
				src.volume = barkVolume;
				src.Play();
			}
		}
	}

	IEnumerator StartBarks()
	{
		//flowchart.ExecuteBlock("BarkTimeTrigger");

		yield return new WaitForSeconds(0);
	}

	#endregion


	public void GoToSpawn()
	{
		if (spawnPoint != null)
		{
			// deactivate the player so the character controller doesn't ovverride teleporting
			playerMove.gameObject.SetActive(false);
			
			playerMove.transform.position = spawnPoint.position;
			playerMove.transform.eulerAngles = spawnPoint.eulerAngles;

			playerMove.gameObject.SetActive(true);
		}
	}
	public void GoToSpawn(Vector3 point, Vector3 rotation = default)
	{
		playerMove.gameObject.SetActive(false);

		playerMove.transform.position = point;
		playerMove.transform.eulerAngles = rotation;

		playerMove.gameObject.SetActive(true);
	}


	GameObject UI(GameObject ui)
	{
		if (ui != null && !GameObject.Find(ui.name))
		{
			return Instantiate(ui, transform);
		}
		else return null;
	}


	bool canPause = true;
	private void Update()
	{
		if (playerMove != null)
		{
			if (PauseMenu != null && SettingsUI != null && !IsGameWin)
			{
				if (Input.GetKeyDown(KeyCode.Escape) && canPause)
				{
					if (!SettingsUI.activeSelf)
					{
						pauseToggel = !pauseToggel;
						SetMouseActive(pauseToggel);
						PauseMenu.SetActive(pauseToggel);

						Time.timeScale = pauseToggel ? 0 : 1;
					}
					else
					{
						SettingsUI.SetActive(false);
						PauseMenu.SetActive(true);
					}
				}
			}

			if (playerMove.transform.position.y <= -50 && !isPlayerDead)
			{
				isPlayerDead = true; // Only apply the damage once
				playerMove.GetComponent<PlayerHealth>().TakeDamage(50000);
			}


			if (Input.GetKeyDown(KeyCode.F5))
			{
				// Quick Save
				QuickSave.Save("quicksaves");
			}

			if (Input.GetKeyDown(KeyCode.F6))
			{
				// Quick Load
				quickSave.QuickLoad();
			}

			if (Input.GetKeyDown(KeyCode.F9))
			{
				Debug.Log("Saving a showcase load. File can be found here:\n<color=blue>" + Application.persistentDataPath + "/showcase</color>");
				quickSave.SaveShowcase();
			}
		}
	}
	bool isPlayerDead;

	public static bool IsPaused
	{
		get { return Cursor.visible; }
	}

	#region Dimensions
	public void ToggleDimension()
	{
		src.clip = dimensionClip;
		src.Play();
		switch (currentDimension)
		{
			case Dimension.Digital:
				SetDimension(Dimension.Real);
				break;

			case Dimension.Real:
				SetDimension(Dimension.Digital);
				break;
		}
	}

	public void SetDimension(Dimension dimension)
	{
		if (dimension != currentDimension)
		{
			currentDimension = dimension;

			if (dimension == Dimension.Digital)
			{
				digitalCam.enabled = false;
				realCam.enabled = true;

				// Play animations for blocks in this dimension
				for (int i = 0; i < digitalWorldObjects.transform.childCount; i++)
				{
					Animator anim = digitalWorldObjects.transform.GetChild(i).GetComponent<Animator>();
					
					if (anim != null && digitalWorldObjects.transform.GetChild(i).CompareTag("StopAnimation"))
					{
						anim.speed = 1;
					}
				}

				// Stop animations for blocks in the other dimension
				for (int i = 0; i < realWorldObjects.transform.childCount; i++)
				{
					Animator anim = realWorldObjects.transform.GetChild(i).GetComponent<Animator>();

					if (anim != null && realWorldObjects.transform.GetChild(i).CompareTag("StopAnimation"))
					{
						anim.speed = 0;
					}
				}


				// ignore collisions from real world
				IgnoreLayer(9, 8); 
			}
			else if (dimension == Dimension.Real)
			{

				digitalCam.enabled = true;
				realCam.enabled = false;

				// Play animations for blocks in this dimension
				for (int i = 0; i < realWorldObjects.transform.childCount; i++)
				{
					Animator anim = realWorldObjects.transform.GetChild(i).GetComponent<Animator>();

					if (anim != null && realWorldObjects.transform.GetChild(i).CompareTag("StopAnimation"))
					{
						anim.speed = 1;
					}
				}

				// Play animations for blocks in the other dimension
				for (int i = 0; i < digitalWorldObjects.transform.childCount; i++)
				{
					Animator anim = digitalWorldObjects.transform.GetChild(i).GetComponent<Animator>();

					if (anim != null && digitalWorldObjects.transform.GetChild(i).CompareTag("StopAnimation"))
					{
						anim.speed = 0;
					}
				}

				// ignore collisions from digital world
				IgnoreLayer(8, 9);
			}
		}
	}

	void IgnoreLayer(int ignoreLayer, int collideLayer)
	{
		// 10 is the Player's layer
		Physics.IgnoreLayerCollision(10, ignoreLayer, true); // true - do ignore these collisions
		Physics.IgnoreLayerCollision(10, collideLayer, false); // false - do not ignore these collisions

		activeLayer = collideLayer;

	}
	#endregion


	void SetMouseActive(bool value)
	{
		Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = value;
	}

	#region Settings
	public void ApplySettings()
	{
		JsonIO.LoadSettings();

		if (playerMove != null)
		{
			GameObject player = playerMove.gameObject;
			// FOV
			foreach (Camera cam in player.GetComponentsInChildren<Camera>())
			{
				cam.fieldOfView = JsonIO.playerSettings.feildOfView;
			}

			// Sensitivity
			MouseSensitivity(JsonIO.playerSettings.lookSensitivity);
			player.GetComponent<PlayerWeapon>().scrollSensitivity = JsonIO.playerSettings.scrollSensitivity;


			// Colors
			Material ctr = player.GetComponent<PlayerController>().digitalEffect;
			ctr.SetColor("_LeadColor", JsonIO.playerSettings.col_outlines);
			ctr.SetColor("_TrailColor", JsonIO.playerSettings.col_background);
			ctr.SetColor("_MidColor", JsonIO.playerSettings.col_outlines * 0.6f); // darken the colour by 60%
			ctr.SetColor("_HBarColor", JsonIO.playerSettings.col_outlines * 0.6f); // darken the colour by 60%


			player.GetComponentInChildren<ScannerEffect>().lineColor = JsonIO.playerSettings.col_outlines;
			player.GetComponentInChildren<ScannerEffect>().secondaryColor = JsonIO.playerSettings.col_background;

			// Accesability
			PlayerController.toggleCrouch = JsonIO.playerSettings.acc_toggelCrouch;
			player.GetComponentInChildren<ScannerEffect>().epilepsySafeMode = JsonIO.playerSettings.acc_epilepticMode;

			src.volume = JsonIO.playerSettings.vol_SoundFX;

			UnityEngine.UI.Image[] corsshair = PlayerHUD.transform.Find("Crosshair").GetComponentsInChildren<UnityEngine.UI.Image>();
			Debug.Log(corsshair.Length);
			for (int i = 0; i < corsshair.Length; i++)
			{
				corsshair[i].color = JsonIO.playerSettings.col_crosshair;
			}
		}


		AccessableColors.digitalColor = JsonIO.playerSettings.col_outlines;
		AccessableColors.textColor = JsonIO.playerSettings.col_text;

		AccessableColors.setColDelagate?.Invoke(JsonIO.playerSettings.col_outlines);
		AccessableColors.setTextColor?.Invoke(JsonIO.playerSettings.col_text);


		for (int i = 0; i < digitalMats.Length; i++)
		{
			digitalMats[i].SetColor("_Color", JsonIO.playerSettings.col_outlines);
			digitalMats[i].SetColor("_BckColor", JsonIO.playerSettings.col_background);
		}

		// Audio
		barkVolume = JsonIO.playerSettings.vol_Barks;
		WeaponBehaviour.volume = JsonIO.playerSettings.vol_SoundFX;
		FootstepsManager.volume = JsonIO.playerSettings.vol_SoundFX;


		// Video
		Screen.fullScreen = JsonIO.playerSettings.isFullscreen;


		for (int i = 0; i < retroProfile.settings.Count; i++)
		{
			retroProfile.settings[i].active = JsonIO.playerSettings.acc_retro;
		}

		if (GameObject.Find("Music"))
		{
			GameObject.Find("Music").GetComponent<AudioSource>().volume = JsonIO.playerSettings.vol_Music;
		}

	}
	
	public void MouseSensitivity(float value)
	{
		playerMove.xMouseSensitivity = value;
		playerMove.yMouseSensitivity = value;
	}



	#endregion

	public void MainMenu()
	{
		SceneLoader.Load_Scene("MainMenu");
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;

		pauseToggel = false;
		SetMouseActive(false);

		// Find the game manager in the scene and set active on the settings ui from there
		// This is because this method is called as a static method from an intasiated gameObject
		FindObjectOfType<GameManager>().PauseMenu.SetActive(false);

	}

	public void DeathMenu()
	{
		Debug.Log("!! DEAD !!");
		Time.timeScale = 0;
		canPause = false;
		SetMouseActive(true);
		DeathMenuUI.SetActive(true);
	}

	public void SettingsMenu()
	{
		FindObjectOfType<GameManager>().PauseMenu.SetActive(false);
		FindObjectOfType<GameManager>().SettingsUI.SetActive(true);
	}

	public void OpenPauseMenu()
	{
		FindObjectOfType<GameManager>().PauseMenu.SetActive(true);
		FindObjectOfType<GameManager>().SettingsUI.SetActive(false);
	}

	public void QuitGame()
	{
		Debug.Log("Quiting");
		Application.Quit();
	}
}
