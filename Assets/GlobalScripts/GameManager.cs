using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

	[Header("Ui and HUDs")]
	public GameObject SettingsUI;
	public GameObject PlayerHUD;
	public GameObject Fungus;

	[Header("Dimension Switching")]
	public static Dimension currentActiveDimension;
	public static int activeLayer;

	public GameObject realWorldObjects;
	public GameObject digitalWorldObjects;

	Camera mainCam;

	ReplacmentShader digitalCam;
	ReplacmentShader realCam;

	static PlayerMovement playerMove;

	private void Start()
	{
		SetMouseActive(false);

		mainCam = Camera.main;
		realCam = mainCam.transform.Find("RealWorldCam").GetComponent<ReplacmentShader>();
		digitalCam = mainCam.transform.Find("DigitalWorldCam").GetComponent<ReplacmentShader>();

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

		UI(SettingsUI);
		UI(PlayerHUD);
		UI(Fungus);

		Debug.Log("!-----------!");
		SettingsUI = transform.GetChild(1).gameObject;
		PlayerHUD = transform.GetChild(2).gameObject;
		Fungus = transform.GetChild(3).gameObject;
		Debug.Log("!-----------!");

		SettingsUI.SetActive(false);

		if (!loadEnemies)
		{
			Debug.LogError("All enemies have been disabled. To enable them, go to _GameManager and set Load Enemies to true.");
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject enemy in enemies)
			{
				enemy.SetActive(false);
			}
		}

		// Set the active dimension to the real
		
		SetDimension(Dimension.Real);
	}

	void UI(GameObject ui)
	{
		if (ui != null && !GameObject.Find(ui.name))
		{
			Instantiate(ui, transform);
		}
	}

	bool settingsToggel;
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
	}

	#region Dimensions
	public void ToggleDimension()
	{
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

				// ignore collisions from real world
				IgnoreLayer(9, 8); 
			}
			else if (dimension == Dimension.Real)
			{

				digitalCam.enabled = true;
				realCam.enabled = false;

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

	public void SetMouseSensitivity(float value)
	{
		playerMove.xMouseSensitivity = value;
		playerMove.yMouseSensitivity = value;
	}

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
