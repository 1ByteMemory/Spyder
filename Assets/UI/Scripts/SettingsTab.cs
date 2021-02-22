using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Setting
{
	FOV,
	Difficulty,
	LookSens,
	ScrollSens,
	Lines,
	Background,
	EnemyOutline,
	FullScrn,
	Res,
	EnemyVoice,
	FX,
	Mus,
	Dia,
	Amb,
	EpiMode,
	Crouch,
	Time,
	Retro
}

public class SettingsTab : MonoBehaviour
{
	public Color OpenColor;

	private Color defaultColor;

	public GameObject[] Menus;
	public GameObject[] TabsLabels;
	public GameObject[] Options;


	private void OnEnable()
	{
		JsonIO.LoadSettings();
		JsonIO.ResetSettings();
		
		defaultColor = TabsLabels[0].GetComponent<Image>().color;
		
		for (int i = 0; i < Menus.Length; i++)
		{
			Menus[i].SetActive(true);
		}

		// General
		Options[0].GetComponent<Slider>().value = JsonIO.playerSettings.feildOfView;
		Options[1].GetComponent<TMP_Dropdown>().value = JsonIO.playerSettings.difficulty;

		// Controls
		Options[2].GetComponent<Slider>().value = JsonIO.playerSettings.lookSensitivity;
		Options[3].GetComponent<Slider>().value = JsonIO.playerSettings.scrollSensitivity;

		// Colors
		Options[4].GetComponent<CPButton>().color = JsonIO.playerSettings.col_outlines;
		Options[5].GetComponent<CPButton>().color = JsonIO.playerSettings.col_background;
		Options[6].GetComponent<CPButton>().color = JsonIO.playerSettings.col_enemyOutline;

		// Audio
		Options[7].GetComponent<Toggle>().isOn = JsonIO.playerSettings.isFullscreen;
		Options[8].GetComponent<TMP_Dropdown>().value = JsonIO.playerSettings.resolution;

		// Video
		Options[9].GetComponent<Slider>().value = JsonIO.playerSettings.vol_Barks;
		Options[10].GetComponent<Slider>().value = JsonIO.playerSettings.vol_SoundFX;
		Options[11].GetComponent<Slider>().value = JsonIO.playerSettings.vol_Music;
		Options[12].GetComponent<Slider>().value = JsonIO.playerSettings.vol_Dialogue;
		Options[13].GetComponent<Slider>().value = JsonIO.playerSettings.vol_Ambience;

		// Accessibility
		Options[14].GetComponent<Toggle>().isOn = JsonIO.playerSettings.acc_epilepticMode;
		Options[15].GetComponent<Toggle>().isOn = JsonIO.playerSettings.acc_toggelCrouch;
		Options[16].GetComponent<Toggle>().isOn = JsonIO.playerSettings.acc_timeSlow;
		Options[17].GetComponent<Toggle>().isOn = JsonIO.playerSettings.acc_retro;

		for (int i = 0; i < Menus.Length; i++)
		{
			Menus[i].SetActive(false);
		}
	}

	public void OpenTab(GameObject tab)
	{
		foreach (GameObject obj in Menus)
		{
			obj.SetActive(false);
		}

		tab.SetActive(true);
	}


	public void TabSelected(Image img)
	{
		foreach (GameObject obj in TabsLabels)
		{
			obj.GetComponent<Image>().color = defaultColor;
		}

		img.color = OpenColor;
	}

	private Setting setting;
	public void SelectedSettings(int settingsIndex)
	{
		setting = (Setting)settingsIndex;
	}


	#region Change Values
	public void ChangeFloat(float value)
	{
		switch (setting)
		{
			case Setting.FOV:
				JsonIO.playerSettings.feildOfView = (int)value;
				break;
			case Setting.LookSens:
				JsonIO.playerSettings.lookSensitivity = value;
				break;
			case Setting.ScrollSens:
				JsonIO.playerSettings.scrollSensitivity = value;
				break;
			case Setting.EnemyVoice:
				JsonIO.playerSettings.vol_Barks = value;
				break;
			case Setting.FX:
				JsonIO.playerSettings.vol_SoundFX = value;
				break;
			case Setting.Mus:
				JsonIO.playerSettings.vol_Music = value;
				break;
			case Setting.Dia:
				JsonIO.playerSettings.vol_Dialogue = value;
				break;
			case Setting.Amb:
				JsonIO.playerSettings.vol_Ambience = value;
				break;
			default:
				Debug.Log("Setting enum not set");
				break;
		}
	}

	public void ChangeInt(int value)
	{
		switch (setting)
		{
			case Setting.Difficulty:
				JsonIO.playerSettings.difficulty = value;
				break;
			case Setting.Res:
				JsonIO.playerSettings.resolution = value;
				break;
			default:
				Debug.Log("Setting enum not set");
				break;
		}
	}

	public void ChangeColor(CPButton buttonValue)
	{
		Color value = buttonValue.color;
		switch (setting)
		{
			case Setting.Lines:
				JsonIO.playerSettings.col_outlines = value;
				break;
			case Setting.Background:
				JsonIO.playerSettings.col_background = value;
				break;
			case Setting.EnemyOutline:
				JsonIO.playerSettings.col_enemyOutline = value;
				break;
			default:
				Debug.Log("Setting enum not set");
				break;
		}
	}

	public void ChangeBool(bool value)
	{
		switch (setting)
		{
			case Setting.FullScrn:
				JsonIO.playerSettings.isFullscreen = value;
				break;
			case Setting.EpiMode:
				JsonIO.playerSettings.acc_epilepticMode = value;
				break;
			case Setting.Crouch:
				JsonIO.playerSettings.acc_toggelCrouch = value;
				break;
			case Setting.Time:
				JsonIO.playerSettings.acc_timeSlow = value;
				break;
			case Setting.Retro:
				JsonIO.playerSettings.acc_retro = value;
				break;
			default:
				Debug.Log("Setting enum not set");
				break;
		}
	}
	#endregion


	public void ApplyChanges()
	{
		GameManager gm = FindObjectOfType<GameManager>();

		if (gm != null)
		{
			gm.ApplySettings();
		}
	}

	public void ResetChanges()
	{
		JsonIO.ResetSettings();
	}
}
