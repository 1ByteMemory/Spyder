using System;
using UnityEngine;

[Serializable]
public class PlayerSettings
{
	// General
	public int feildOfView;
	public int difficulty;

	//Controls
	public float lookSensitivity;
	public float scrollSensitivity;

	// Colors
	public Color outlines;
	public Color background;
	public Color enemyOutline;

	// Video
	public bool isFullscreen;
	public int resolution;

	// Audio
	public float enemyVoice;
	public float soundFX;
	public float music;
	public float dialogue;
	public float ambience;

	// Accessability
	public bool epilepticMode;
	public bool toggelCrouch;
	public bool timeSlow;


	public string SaveToString()
	{
		return JsonUtility.ToJson(this);
	}

	public static PlayerSettings CreateFromJson(string jsonString)
	{
		return JsonUtility.FromJson<PlayerSettings>(jsonString);
	}

}
