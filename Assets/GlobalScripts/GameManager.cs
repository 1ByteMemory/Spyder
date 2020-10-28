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
