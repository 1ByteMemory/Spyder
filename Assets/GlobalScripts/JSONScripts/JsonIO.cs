﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class JsonIO : MonoBehaviour
{
	public static PlayerSettings playerSettings = new PlayerSettings();


	public static void ResetSettings()
	{
		playerSettings.version = GameManager.version;

		// General
		playerSettings.feildOfView = 70;
		playerSettings.difficulty = 1;

		//Controls
		playerSettings.lookSensitivity = 150;
		playerSettings.scrollSensitivity = 10;

		// Colors
		playerSettings.col_outlines = Color.green;
		playerSettings.col_background = Color.black;
		playerSettings.col_text = Color.white;
		playerSettings.col_crosshair = Color.white;

		// Video
		playerSettings.isFullscreen = true;
		playerSettings.resolution = 0;

		// Audio
		playerSettings.vol_Barks = 1;
		playerSettings.vol_SoundFX = 1;
		playerSettings.vol_Music = 1;
		playerSettings.vol_Dialogue = 1;
		playerSettings.vol_Ambience = 1;

		// Accessability
		playerSettings.acc_epilepticMode = false;
		playerSettings.acc_toggelCrouch = false;
		playerSettings.acc_timeSlow = false;
		playerSettings.acc_retro = true;
	}

	public static void LoadSettings()
	{
		StreamReader reader;
		
		try
		{
			reader = new StreamReader(Application.dataPath + "/Resources/config.json");
			playerSettings = PlayerSettings.CreateFromJson(reader.ReadToEnd());
			reader.Close();
			Debug.Log("Settings Loaded");

			if (GameManager.version != playerSettings.version)
			{
				Debug.Log("Config version is not up to date, creating new config");
				ResetSettings();
				SaveSettings(playerSettings);
			}
		}
		catch (FileNotFoundException)
		{
			Debug.Log("Settings file not found, creating a new file...");
			ResetSettings();
			SaveSettings(playerSettings);
		}
		catch (DirectoryNotFoundException)
		{
#if (UNITY_EDITOR)
			Debug.Log("Resources directory not found, creating Resources Folder");
			AssetDatabase.CreateFolder("Assets", "Resources");
			ResetSettings();
			SaveSettings(playerSettings);
#endif
		}
	}

	public static void SaveSettings(PlayerSettings settings)
	{
		string jsonTextFile = settings.SaveToString();

		StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/config.json");

		writer.WriteLine(jsonTextFile);
		writer.Close();

		Debug.Log("Saved Settings");
	}
}
