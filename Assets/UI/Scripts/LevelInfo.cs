using UnityEngine;

[System.Serializable]
public class LevelInfo
{
    public string title;
    public string sceneName;
	public bool abilityUnlocked;
    public Vector3 spawnPoint;
    public Weapon[] availableWeapons;
}
