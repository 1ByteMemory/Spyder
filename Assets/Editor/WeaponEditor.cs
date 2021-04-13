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
	SerializedProperty hitLayers;

	SerializedProperty model;
	SerializedProperty bullet;
	SerializedProperty bulletIndex;
	SerializedProperty muzzleFlash;

	SerializedProperty fireAudio;
	SerializedProperty reloadAudio;

	SerializedProperty bulletCount;
	SerializedProperty bulletDensity;

	SerializedProperty holdToFire;
	SerializedProperty firingTime;
	SerializedProperty reloadTime;
	SerializedProperty shotSpeed;
	SerializedProperty range;
	SerializedProperty damage;
	SerializedProperty dmgMult;

	SerializedProperty isAmmoInf;
	SerializedProperty isClipInf;

	SerializedProperty maxAmmo;
	SerializedProperty maxClip;

	SerializedProperty ammo;
	SerializedProperty clip;

	public WeaponType _weapontype;


	private void OnEnable()
	{
		weapon = (Weapon)target;

		GetTarget = new SerializedObject(weapon);
	}


	public override void OnInspectorGUI()
	{
		
		#region Styles and Layouts
		GUIStyle subtitle = new GUIStyle
		{
			fontStyle = FontStyle.Bold,
		};

		GUIStyle title = new GUIStyle
		{
			fontStyle = FontStyle.Bold,
			fontSize = 15
		};

		#endregion

		#region serializedObjects
		Name = serializedObject.FindProperty("Name");
		weaponType = serializedObject.FindProperty("weaponType");
		hitLayers = serializedObject.FindProperty("hitLayers");

		model = serializedObject.FindProperty("model");
		bullet = serializedObject.FindProperty("bullet");
		bulletIndex = serializedObject.FindProperty("bulletIndex");
		muzzleFlash = serializedObject.FindProperty("muzzleFlash");

		fireAudio = serializedObject.FindProperty("fireAudio");
		reloadAudio = serializedObject.FindProperty("reloadAudio");
		
		bulletCount = serializedObject.FindProperty("bulletCount");
		bulletDensity = serializedObject.FindProperty("bulletDensity");

		holdToFire = serializedObject.FindProperty("holdToFire");
		firingTime = serializedObject.FindProperty("firingTime");
		reloadTime = serializedObject.FindProperty("reloadTime");
		shotSpeed = serializedObject.FindProperty("shotSpeed");
		range = serializedObject.FindProperty("range");
		damage = serializedObject.FindProperty("damage");
		dmgMult = serializedObject.FindProperty("dmgMult");


		isAmmoInf = serializedObject.FindProperty("isAmmoInf");
		isClipInf = serializedObject.FindProperty("isClipInf");
		maxAmmo = serializedObject.FindProperty("maxAmmo");
		maxClip = serializedObject.FindProperty("maxClip");
		ammo = serializedObject.FindProperty("startingAmmo");
		clip = serializedObject.FindProperty("startingClip");
		#endregion

		GetTarget.Update();

		EditorGUILayout.LabelField("Weapon", title);
		Name.stringValue = EditorGUILayout.TextField("Name", Name.stringValue);
		//_weapontype = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", _weapontype);
		weaponType.enumValueIndex = (int)(WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weapon.weaponType);
		_weapontype = (WeaponType)weaponType.enumValueIndex;

		if (_weapontype == WeaponType.HitScan)
		{
			string[] layerOptions = new string[32];
			for (int i = 0; i < 32; i++)
			{

				layerOptions[i] = LayerMask.LayerToName(i);
			}
			
			hitLayers.intValue = EditorGUILayout.MaskField("Hit Layers", hitLayers.intValue, layerOptions);

		}

		

		#region Gun and Bullet Model
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Gun and Bullet Model", subtitle);

		model.objectReferenceValue = EditorGUILayout.ObjectField("Gun Model", model.objectReferenceValue, typeof(GameObject), false);

		string[] options = new string[0];
		if ((GameObject)model.objectReferenceValue != null)
		{
			GameObject obj = (GameObject)model.objectReferenceValue;
			options = new string[obj.transform.childCount];
		
			if (obj.transform.childCount > 0)
			{
				for (int i = 0; i < options.Length; i++)
				{
					options[i] = obj.transform.GetChild(i).name;
				}
			}
		}

		bullet.objectReferenceValue = EditorGUILayout.ObjectField("Bullet", bullet.objectReferenceValue, typeof(GameObject), false);
		muzzleFlash.objectReferenceValue = EditorGUILayout.ObjectField("Muzzle Flash", muzzleFlash.objectReferenceValue, typeof(GameObject), false);
		bulletIndex.intValue = EditorGUILayout.Popup("Bullet Origin:", bulletIndex.intValue, options, EditorStyles.popup);


		fireAudio.objectReferenceValue = EditorGUILayout.ObjectField("Firing Sound", fireAudio.objectReferenceValue, typeof(AudioClip), false);
		reloadAudio.objectReferenceValue = EditorGUILayout.ObjectField("Reloading Sound", reloadAudio.objectReferenceValue, typeof(AudioClip), false);
		
		
		#endregion

		#region Bullet Spread and Density
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Bullet Spread and Density", subtitle);

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
		EditorGUILayout.LabelField("Stats", subtitle);

		
		holdToFire.boolValue = EditorGUILayout.Toggle("Hold To Fire", holdToFire.boolValue);
		
		firingTime.floatValue = EditorGUILayout.FloatField("Time between shots", firingTime.floatValue);
		reloadTime.floatValue = EditorGUILayout.FloatField("Reloading Time", reloadTime.floatValue);
		if (_weapontype == WeaponType.Projectile)
			shotSpeed.floatValue = EditorGUILayout.FloatField("Bullet travel speed", shotSpeed.floatValue);
		range.floatValue = EditorGUILayout.FloatField("Range", range.floatValue);
		damage.intValue = EditorGUILayout.IntField("Damage per bullet", damage.intValue);
		dmgMult.floatValue = EditorGUILayout.FloatField("Dimension Multiplier", dmgMult.floatValue);

		firingTime.floatValue = firingTime.floatValue < 0 ? 0 : firingTime.floatValue;
		shotSpeed.floatValue = shotSpeed.floatValue < 0 ? 0 : shotSpeed.floatValue;
		range.floatValue = range.floatValue < 0 ? 0 : range.floatValue > 100 ? 100 : range.floatValue;
		#endregion

		#region Ammo and Clip
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Ammo and Clip", subtitle);

		isAmmoInf.boolValue = EditorGUILayout.Toggle("Is Ammo Infinate", isAmmoInf.boolValue);
		if (!isAmmoInf.boolValue)
		{
			maxAmmo.intValue = EditorGUILayout.IntField("Max Ammo", maxAmmo.intValue);
			ammo.intValue = EditorGUILayout.IntField("Starting Ammo", ammo.intValue);
		}
		else
		{
			maxAmmo.intValue = 1;
			ammo.intValue = 1;
		}

		EditorGUILayout.Space();

		isClipInf.boolValue = EditorGUILayout.Toggle("Is Clip Infinate", isClipInf.boolValue);
		if (!isClipInf.boolValue)
		{
			maxClip.intValue = EditorGUILayout.IntField("Max Clip", maxClip.intValue);
			clip.intValue = EditorGUILayout.IntField("Starting Ammo in Clip", clip.intValue);
		}
		else
		{
			maxClip.intValue = 1;
			clip.intValue = 1;
		}

		maxAmmo.intValue = maxAmmo.intValue < 1 ? 1 : maxAmmo.intValue;
		maxClip.intValue = maxClip.intValue < 1 ? 1 : maxClip.intValue;

		// Keep the starting ammo and clip inside the range of 0 - maxAmmo/maxClip respectivly
		ammo.intValue = ammo.intValue < 0 ? 0 : ammo.intValue > maxAmmo.intValue ? maxAmmo.intValue : ammo.intValue;
		clip.intValue = clip.intValue < 0 ? 0 : clip.intValue > maxClip.intValue ? maxClip.intValue : clip.intValue;
		#endregion

		// Save the changes to the scriptable object
		serializedObject.ApplyModifiedProperties();
	}
}
