using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelInfo
{
    public string title;
    public string sceneName;
    public Sprite image;
    public Vector3 spawnPoint;
    public Weapon[] availableWeapons;

}
