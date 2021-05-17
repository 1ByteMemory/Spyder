using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
	public string version;

	// General
	public int feildOfView;
	public int difficulty;

	//Controls
	public float lookSensitivity;
	public float scrollSensitivity;

	// Colors
	public Color col_outlines;
	public Color col_background;
	public Color col_text;
	public Color col_crosshair;

	// Video
	public bool isFullscreen;
	public int resolution;

	// Audio
	public float vol_Barks;
	public float vol_SoundFX;
	public float vol_Music;
	public float vol_Dialogue;
	public float vol_Ambience;

	// Accessability
	public bool acc_epilepticMode;
	public bool acc_toggelCrouch;
	public bool acc_timeSlow;
	public bool acc_retro;

	public string SaveToString()
	{
		return JsonUtility.ToJson(this, true);
	}

	public static PlayerSettings CreateFromJson(string jsonString)
	{
		return JsonUtility.FromJson<PlayerSettings>(jsonString);
	}

}
