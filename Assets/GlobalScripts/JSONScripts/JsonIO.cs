using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			playerSettings.feildOfView = 90;
			playerSettings.outlines = Color.green;

			
			SaveSettings(playerSettings);
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			LoadSettings();

		}
		text.text = playerSettings.feildOfView.ToString();
	}

	public void LoadSettings()
	{
		Debug.Log("Loading Settings");
		var jsonTextFile = Resources.Load<TextAsset>("settings");
		playerSettings = PlayerSettings.CreateFromJson(jsonTextFile.ToString());
		Debug.Log("Settings Loaded");
		Debug.Log(playerSettings.ToString());
	}

	public void SaveSettings(PlayerSettings settings)
	{
		string jsonTextFile = settings.SaveToString();

		Debug.Log("Saving Settings");
		StreamWriter writer = new StreamWriter(Application.dataPath + "/Resources/settings.json");

		Debug.Log("Writing line");
		writer.WriteLine(jsonTextFile);
		writer.Close();
		

	}
}
