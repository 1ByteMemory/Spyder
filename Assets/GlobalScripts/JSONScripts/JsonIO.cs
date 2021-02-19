using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class JsonIO : MonoBehaviour
{
    public PlayerSettings playerSettings = new PlayerSettings();

	public Text text;

	private void Start()
	{
		LoadSettings();
	}

	public void Update()
	{



		if (Input.GetKeyDown(KeyCode.S))
		{
			playerSettings.feildOfView = 90;
			playerSettings.outlines = Color.green;

			
			SaveSettings(playerSettings);
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadSettings();

		}
		text.text = playerSettings.feildOfView.ToString();
	}

	public void ResetSettings()
	{
		// General
		playerSettings.feildOfView = 70;
		playerSettings.difficulty = 1;

		//Controls
		playerSettings.lookSensitivity = 150;
		playerSettings.scrollSensitivity = 10;

		// Colors
		playerSettings.outlines = Color.green;
		playerSettings.background = Color.black;
		playerSettings.enemyOutline = Color.red;

		// Video
		playerSettings.isFullscreen = true;
		playerSettings.resolution = 0;

		// Audio
		playerSettings.enemyVoice = 1;
		playerSettings.soundFX = 1;
		playerSettings.music = 1;
		playerSettings.dialogue = 1;
		playerSettings.ambience = 1;

		// Accessability
		playerSettings.epilepticMode = false;
		playerSettings.toggelCrouch = false;
		playerSettings.timeSlow = false;
}

	public void LoadSettings()
	{
		StreamReader reader;
		
		try
		{
			reader = new StreamReader(Application.dataPath + "/Resources/settings.json");
			playerSettings = PlayerSettings.CreateFromJson(reader.ReadToEnd());
			reader.Close();
			Debug.Log("Settings Loaded");
		}
		catch (FileNotFoundException)
		{
			Debug.Log("Settings file not found, creating a new file...");
			ResetSettings();
			SaveSettings(playerSettings);
		}
		catch (DirectoryNotFoundException)
		{
			Debug.Log("Resources directory not found, creating Resources Folder");
			AssetDatabase.CreateFolder("Assets", "Resources");
			ResetSettings();
			SaveSettings(playerSettings);

		}
	}

	public void SaveSettings(PlayerSettings settings)
	{
		string jsonTextFile = settings.SaveToString();

		StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/settings.json");

		writer.WriteLine(jsonTextFile);
		writer.Close();

		Debug.Log("Saved Settings");
	}
}
