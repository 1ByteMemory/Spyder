using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("Save")]
public class SaveInfo
{
    public string title = string.Empty;
    public string sceneName = string.Empty;
	public bool abilityUnlocked = false;
	public bool[] foundKeys = new bool[0];
	public int dimension = 0;
    public Vector3 spawnPoint = Vector3.zero;

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
    public void SaveToXml(string path, SaveInfo save)
	{
        var serializer = new XmlSerializer(typeof(SaveInfo));
		using (var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, save);
		}
	}

    public static SavesContainer LoadFromXml(string path)
	{
		var serializer = new XmlSerializer(typeof(SavesContainer));
		using (var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as SavesContainer;
		}
	}
}
