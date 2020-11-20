using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement; // ENGINE


public class GameManager : MonoBehaviour
{
	[Header("Scene Loader")]
	public bool addScene;
	bool isLoaded;
	public string sceneToLoad;
	public bool loadEnemies = true;

	[Header("")]
	public GameObject SettingsUI;

	private void Awake()
	{
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
	}


	private void Start()
	{
		SetMouseActive(false);

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

	void SetMouseActive(bool value)
	{
		Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = value;
	}

}
