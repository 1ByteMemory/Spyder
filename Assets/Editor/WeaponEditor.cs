using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Weapon))]
[CanEditMultipleObjects]
public class WeaponEditor : Editor
{
	Weapon weapon;
	SerializedObject GetTarget;

	SerializedProperty Name;
	SerializedProperty weaponType;

	SerializedProperty flags;
	SerializedProperty entities;

	SerializedProperty model;
	SerializedProperty bullet;

	SerializedProperty bulletOrigin;
	SerializedProperty bulletCount;
	SerializedProperty bulletDensity;

	SerializedProperty holdToFire;
	SerializedProperty firingTime;
	SerializedProperty shotSpeed;
	SerializedProperty range;
	SerializedProperty damage;

	SerializedProperty maxAmmo;
	SerializedProperty maxClip;

	SerializedProperty ammo;
	SerializedProperty clip;

	public WeaponType _weapontype;

	public int index = 0;
	string[] options;

	private void OnEnable()
	{
		weapon = (Weapon)target;

		GetTarget = new SerializedObject(weapon);
	}


	public override void OnInspectorGUI()
	{
		#region Styles and Layouts
		GUIStyle bold = new GUIStyle
		{
			fontStyle = FontStyle.Bold
		};

		#endregion

		#region serializedObjects
		Name = serializedObject.FindProperty("Name");
		weaponType = serializedObject.FindProperty("weaponTpye");
		flags = serializedObject.FindProperty("flags");
		entities = serializedObject.FindProperty("entities");

		model = serializedObject.FindProperty("model");
		bulletOrigin = serializedObject.FindProperty("bulletOrigin");
		bullet = serializedObject.FindProperty("bullet");
		
		bulletCount = serializedObject.FindProperty("bulletCount");
		bulletDensity = serializedObject.FindProperty("bulletDensity");

		holdToFire = serializedObject.FindProperty("holdToFire");
		firingTime = serializedObject.FindProperty("firingTime");
		shotSpeed = serializedObject.FindProperty("shotSpeed");
		range = serializedObject.FindProperty("range");
		damage = serializedObject.FindProperty("damage");

		maxAmmo = serializedObject.FindProperty("maxAmmo");
		maxClip = serializedObject.FindProperty("maxClip");
		ammo = serializedObject.FindProperty("ammo");
		clip = serializedObject.FindProperty("clip");
		#endregion

		GetTarget.Update();

		EditorGUILayout.LabelField("Weapon", bold);
		Name.stringValue = EditorGUILayout.TextField("Name", Name.stringValue);
		_weapontype = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", _weapontype);

		string[] options = new string[entities.arraySize];
		for (int i = 0; i < options.Length; i++)
		{
			options[i] = entities.GetArrayElementAtIndex(i).stringValue;
		}

		flags.intValue = EditorGUILayout.MaskField("Useable by:", flags.intValue, options);

		#region Gun and Bullet Model
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Gun and Bullet Model", bold);

		model.objectReferenceValue = EditorGUILayout.ObjectField("Gun Model", model.objectReferenceValue, typeof(GameObject), false);

		if ((GameObject)model.objectReferenceValue != null)
		{
			GameObject obj = (GameObject)model.objectReferenceValue;
			options = new string[obj.transform.childCount + 1];
		
			options[0] = model.objectReferenceValue.name;
			if (obj.transform.childCount > 0)
			{
				for (int i = 1; i < options.Length; i++)
				{
					options[i] = obj.transform.GetChild(i - 1).name;
				}
			}
		}

		bullet.objectReferenceValue = EditorGUILayout.ObjectField("Bullet", bullet.objectReferenceValue, typeof(GameObject), false);
		index = EditorGUILayout.Popup("Bullet Origin:", index, options, EditorStyles.popup);
		#endregion

		#region Bullet Spread and Density
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Bullet Spread and Density", bold);

		Vector2Int xy = EditorGUILayout.Vector2IntField("Bullet Count", bulletCount.vector2IntValue);
		xy.x = xy.x < 1 ? 1 : xy.x;
		xy.y = xy.y < 1 ? 1 : xy.y;
		bulletCount.vector2IntValue = xy;


		bulletDensity.floatValue = EditorGUILayout.FloatField("Bullets per Unit", bulletDensity.floatValue);
		bulletDensity.floatValue = bulletDensity.floatValue < 0.1f ? 0.1f : bulletDensity.floatValue;
		#endregion

		#region Stats
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Stats", bold);

		holdToFire.boolValue = EditorGUILayout.Toggle("Hold To Fire", holdToFire.boolValue);
		
		
		firingTime.floatValue = EditorGUILayout.FloatField("Time between shots", firingTime.floatValue);
		shotSpeed.floatValue = EditorGUILayout.FloatField("Bullet travel speed", shotSpeed.floatValue);
		range.floatValue = EditorGUILayout.FloatField("Range", range.floatValue);
		damage.intValue = EditorGUILayout.IntField("Damage per bullet", damage.intValue);

		firingTime.floatValue = firingTime.floatValue < 0 ? 0 : firingTime.floatValue;
		shotSpeed.floatValue = shotSpeed.floatValue < 0 ? 0 : shotSpeed.floatValue;
		range.floatValue = range.floatValue < 0 ? 0 : range.floatValue;
		#endregion

		#region Ammo and Clip
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Ammo and Clip", bold);

		maxAmmo.intValue = EditorGUILayout.IntField("Max Ammo", maxAmmo.intValue);
		maxClip.intValue = EditorGUILayout.IntField("Max Clip", maxClip.intValue);

		ammo.intValue = EditorGUILayout.IntField("Starting Ammo", ammo.intValue);
		clip.intValue = EditorGUILayout.IntField("Starting Ammo in Clip", clip.intValue);

		maxAmmo.intValue = maxAmmo.intValue < 1 ? 1 : maxAmmo.intValue;
		maxClip.intValue = maxClip.intValue < 1 ? 1 : maxClip.intValue;

		ammo.intValue = ammo.intValue < 0 ? 0 : ammo.intValue;
		clip.intValue = clip.intValue < 0 ? 0 : clip.intValue;
		#endregion

		// Save the changes to the scriptable object
		serializedObject.ApplyModifiedProperties();
	}
}
