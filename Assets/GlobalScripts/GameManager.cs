﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public enum Dimension
{
	Digital,
	Real
}

public class GameManager : MonoBehaviour
{
	[Header("Scene Loader")]
	//public bool addScene;
	bool isLoaded;
	//public string sceneToLoad;
	public bool loadEnemies = true;

	[Header("UI and HUDs")]
	public GameObject SettingsUI;
	public GameObject PlayerHUD;
	public GameObject Fungus;

	public static Dimension currentActiveDimension;
	public static int activeLayer;

	[Header("Dimension Switching")]
	public AudioClip dimensionClip;
	private AudioSource src;

	public GameObject realWorldObjects;
	public GameObject digitalWorldObjects;


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
	private Flowchart flowchart;

	public Material digitalMats;

	private void Start()
	{
		SetMouseActive(false);

		mainCam = Camera.main;

		realCam = mainCam.transform.Find("RealWorldCam").gameObject.GetComponent<ReplacmentShader>();
		digitalCam = mainCam.transform.Find("DigitalWorldCam").gameObject.GetComponent<ReplacmentShader>();
		

		playerMove = FindObjectOfType<PlayerMovement>();

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

		UI(SettingsUI);
		UI(PlayerHUD);
		UI(Fungus);

		SettingsUI = transform.Find(SettingsUI.name + "(Clone)").gameObject;
		PlayerHUD = transform.Find(PlayerHUD.name + "(Clone)").gameObject;
		Fungus = transform.Find(Fungus.name + "(Clone)").gameObject;

		flowchart = Fungus.GetComponentInChildren<Flowchart>();

		

		StartCoroutine(StartBarks());

		SettingsUI.SetActive(false);

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

		if (spawnAtSpawnPoint) GoToSpawn();

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
		flowchart.ExecuteBlock("BarkTimeTrigger");

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

	void UI(GameObject ui)
	{
		if (ui != null && !GameObject.Find(ui.name))
		{
			Instantiate(ui, transform);
		}
	}

	static bool settingsToggel;
	private void Update()
	{
		if (SettingsUI != null)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				settingsToggel = !settingsToggel;
				SetMouseActive(settingsToggel);
				SettingsUI.SetActive(settingsToggel);

				Time.timeScale = settingsToggel ? 0 : 1;
			}
		}

		if (playerMove.transform.position.y <= -100)
		{
			Debug.LogError("Player was below -100, teleporting to spawn");
			GoToSpawn();
		}
	}

	#region Dimensions
	public void ToggleDimension()
	{
		src.clip = dimensionClip;
		src.Play();
		switch (currentActiveDimension)
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
		if (dimension != currentActiveDimension)
		{
			currentActiveDimension = dimension;

			if (dimension == Dimension.Digital)
			{
				digitalCam.enabled = false;
				realCam.enabled = true;

				// Play animations for blocks in this dimension
				for (int i = 0; i < digitalWorldObjects.transform.childCount; i++)
				{
					Animator anim = digitalWorldObjects.transform.GetChild(i).GetComponent<Animator>();
					
					if (anim != null)
					{
						anim.speed = 1;
					}
				}

				// Stop animations for blocks in the other dimension
				for (int i = 0; i < realWorldObjects.transform.childCount; i++)
				{
					Animator anim = realWorldObjects.transform.GetChild(i).GetComponent<Animator>();

					if (anim != null)
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

					if (anim != null)
					{
						anim.speed = 1;
					}
				}

				// Play animations for blocks in the other dimension
				for (int i = 0; i < digitalWorldObjects.transform.childCount; i++)
				{
					Animator anim = digitalWorldObjects.transform.GetChild(i).GetComponent<Animator>();

					if (anim != null)
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

		digitalMats.SetColor("_Color", JsonIO.playerSettings.col_outlines);
		digitalMats.SetColor("_BckColor", JsonIO.playerSettings.col_background);

		// Audio
		barkVolume = JsonIO.playerSettings.vol_Barks;
		WeaponBehaviour.volume = JsonIO.playerSettings.vol_SoundFX;
		FootstepsManager.volume = JsonIO.playerSettings.vol_SoundFX;
		src.volume = JsonIO.playerSettings.vol_SoundFX;


		// Accesability
		PlayerController.toggleCrouch = JsonIO.playerSettings.acc_toggelCrouch;
	}
	
	public void MouseSensitivity(float value)
	{
		playerMove.xMouseSensitivity = value;
		playerMove.yMouseSensitivity = value;
	}



	#endregion

	public void ResumeGame()
	{
		Time.timeScale = 1;

		settingsToggel = false;
		SetMouseActive(false);

		// Find the game manager in the scene and set active on the settings ui from there
		// This is because this method is called as a static method from an intasiated gameObject
		FindObjectOfType<GameManager>().SettingsUI.SetActive(false);

	}

	public void QuitGame()
	{
		Debug.Log("Quiting");
		Application.Quit();
	}
}
