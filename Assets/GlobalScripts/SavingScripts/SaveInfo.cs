using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;

[XmlRoot("Save")]
public class SaveInfo
{
    public string title = string.Empty;
    public string sceneName = string.Empty;
	public bool abilityUnlocked = false;
	public bool[] foundKeys = new bool[0];
	public int health = 0;
	public int dimension = 0;
    public Vector3 spawnPoint = Vector3.zero;
	public Vector3 spawnRotation = Vector3.zero;

	public Gun[] availableWeapons = new Gun[0];
}

public class Gun
{
	[XmlAttribute("name")]
	public string name;
	[XmlAttribute("ammo")]
	public int ammo = 0;
	[XmlAttribute("clip")]
	public int clip;

	public Gun(string name, int ammo, int clip)
	{
		this.name = name;
		this.ammo = ammo;
		this.clip = clip;
	}

	public Gun()
	{
		name = null;
		ammo = 0;
		clip = 0;
	}
}

[XmlRoot("Saves Collection")]
public class SavesContainer
{
	public List<SavesContainer> SavesList = new List<SavesContainer>();


    public void SaveToXml(string path, SaveInfo save)
	{
        var serializer = new XmlSerializer(typeof(SaveInfo));
		using (var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, save);
		}
	}

    public static SaveInfo LoadFromXml(string path)
	{
		var serializer = new XmlSerializer(typeof(SaveInfo));
		using (var stream = new FileStream(path, FileMode.Open))
		{
			SaveInfo info = serializer.Deserialize(stream) as SaveInfo;
			return info;
		}
	}

	public static SaveInfo[] LoadFromXmls(string folderPath)
	{
		SaveInfo[] saves = new SaveInfo[0];
		if (Directory.Exists(folderPath))
		{
			string[] files = Directory.GetFiles(folderPath, "*.xml");
			saves = new SaveInfo[files.Length];

			// loop through all files in folder
			for (int i = 0; i < files.Length; i++)
			{
				saves[i] = LoadFromXml(files[i]);
			}
		}
		return saves;
	}

	public static void DeleteFolder(string folder)
	{
		string path = Path.Combine(Application.persistentDataPath, folder);
		if (Directory.Exists(path))
		{
			Debug.Log("Deleting");
			Directory.Delete(path, true);
		}
	}
}
