using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement; // ENGINE

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

	[Header("")]
	public GameObject SettingsUI;


	[Header("Dimension Switching")]
	public static Dimension currentActiveDimension;
	public static int activeLayer;

	public GameObject realWorldObjects;
	public GameObject digitalWorldObjects;

	Camera mainCam;

	GameObject digitalCam;
	GameObject realCam;

	private void Start()
	{
		SetMouseActive(false);

		mainCam = Camera.main;
		realCam = mainCam.transform.Find("RealWorldCam").gameObject;
		digitalCam = mainCam.transform.Find("DigitalWorldCam").gameObject;

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


		if (SettingsUI != null)
		{
			SettingsUI.SetActive(false);
		}

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
			}
		}
	}

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
				digitalCam.SetActive(false);
				realCam.SetActive(true);

				// ignore collisions from real world
				IgnoreLayer(9, 8); 
			}
			else if (dimension == Dimension.Real)
			{

				digitalCam.SetActive(true);
				realCam.SetActive(false);

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

	void SetMouseActive(bool value)
	{
		Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = value;
	}
}
