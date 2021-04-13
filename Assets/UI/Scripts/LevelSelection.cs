using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SaveType
{
	Quick,
	Auto
}

[RequireComponent(typeof(SceneLoader))]
public class LevelSelection : MonoBehaviour
{
    public ScrollRect showCaseView;
	public ScrollRect quickSavesView;
	public ScrollRect autoSavesView;

	public int maxQuickSaves = 10;
	public int maxAutoSaves = 5;

	SceneLoader sceneLoader;

	public Vector2 border;
	public Vector2 cardGap;

	public float textHieght = 39;

    public SaveInfo[] showCaseLevels = new SaveInfo[0];

	private List<SaveInfo> quickSaves = new List<SaveInfo>();
	private List<SaveInfo> autoSaves = new List<SaveInfo>();

	private SaveInfo[] AllSaves;

	public void NewSave(SaveInfo save, SaveType saveType)
	{
		switch (saveType)
		{
			case SaveType.Quick:
				quickSaves.Insert(0, save);
				CreateList(quickSavesView, quickSaves.ToArray());
				break;
			case SaveType.Auto:
				autoSaves.Insert(0, save);
				CreateList(autoSavesView, autoSaves.ToArray());
				break;
		}
	}

	public static void DeleteSave(SaveInfo save, SaveType saveType)
	{
		switch (saveType)
		{
			case SaveType.Quick:
				break;
			case SaveType.Auto:
				break;
		}
	}

	private void OnEnable()
	{
		sceneLoader = GetComponent<SceneLoader>();

		showCaseLevels = SavesContainer.LoadFromXmls(Path.Combine(Application.persistentDataPath, "showcase"));

		SaveInfo[] QuickSaves = SavesContainer.LoadFromXmls(Path.Combine(Application.persistentDataPath, "quicksaves"));
		for (int i = 0; i < QuickSaves.Length; i++)
		{
			quickSaves.Add(QuickSaves[i]);
		}

		SaveInfo[] AutoSaves = SavesContainer.LoadFromXmls(Path.Combine(Application.persistentDataPath, "autosaves"));
		for (int i = 0; i < AutoSaves.Length; i++)
		{
			autoSaves.Add(AutoSaves[i]);
		}
		
		showCaseView.content.sizeDelta = new Vector2(0, showCaseLevels.Length * textHieght);
		quickSavesView.content.sizeDelta = new Vector2(0, quickSaves.Count * textHieght);
		autoSavesView.content.sizeDelta = new Vector2(0, autoSaves.Count * textHieght);

		AllSaves = new SaveInfo[showCaseLevels.Length + quickSaves.Count + autoSaves.Count];
		for (int i = 0; i < showCaseLevels.Length; i++)
		{
			AllSaves[i] = showCaseLevels[i];
		}
		for (int i = 0; i < quickSaves.Count; i++)
		{
			AllSaves[i + showCaseLevels.Length] = quickSaves[i];
		}
		for (int i = 0; i < autoSaves.Count; i++)
		{
			AllSaves[i + showCaseLevels.Length + quickSaves.Count] = autoSaves[i];
		}

		CreateList(showCaseView, showCaseLevels);
		CreateList(quickSavesView, quickSaves.ToArray());
		CreateList(autoSavesView, autoSaves.ToArray());
	}

	public void CreateList(ScrollRect scrollRect, SaveInfo[] levelList)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			Destroy(scrollRect.content.GetChild(i).gameObject);
		}

		for (int i = 0; i < levelList.Length; i++)
		{
			// Create empty rect
			RectTransform save = CreateRect(levelList[i].title, scrollRect.content);
			
			// Set Sizes and position
			save.anchorMin = new Vector2(0, 1.0f - (1.0f / levelList.Length));
			save.anchorMax = new Vector2(0.94f, 1);
			save.sizeDelta = new Vector2(0, 0);
			save.anchoredPosition = new Vector2(0, -i * textHieght);

			// Create Title
			RectTransform title = CreateRect("Title", save);
			TextMeshProUGUI text = title.gameObject.AddComponent<TextMeshProUGUI>();
			text.text = levelList[i].title;
			text.color = Color.white; // Needs to be white so the button can change it's color tint
			text.enableAutoSizing = true;
			text.alignment = TextAlignmentOptions.Center;
			text.margin = new Vector4(0, 8, 0, 8);

			title.anchorMin = Vector2.zero;
			title.anchorMax = new Vector2(1, 1);
			title.sizeDelta = new Vector2(0, 0);

			// Button
			Button button = save.gameObject.AddComponent<Button>();
			button.targetGraphic = text;

			// Change button colors
			ColorBlock block = button.colors;
			block.normalColor = new Color(0.1f, 0.12f, 0.12f);
			block.highlightedColor = new Color(0.7f, 0.7f, 0.7f);
			block.selectedColor = new Color(0.4f, 0.4f, 0.4f);
			button.colors = block;

			// Add LoadLevel Method to OnClick event

			string tempTitle = levelList[i].title;
			button.onClick.AddListener(delegate { LoadScene(tempTitle); });
		}

	}

	public void LoadScene(string name)
	{
		Debug.Log(name);
		for (int i = 0; i < AllSaves.Length; i++)
		{
			if (AllSaves[i].title == name)
			{
				QuickSave.mostRecentLoad = AllSaves[i];

				GameManager.loadedFromSave = true;
				GameManager.loadedSpawnPosition = AllSaves[i].spawnPoint;

				sceneLoader.LoadScene(AllSaves[i].sceneName);
			}
		}
	}


	private RectTransform CreateRect(string name, RectTransform parent)
	{
		RectTransform rect = new GameObject().AddComponent<RectTransform>();
		rect.name = name;
		rect.SetParent(parent, false);
		return rect;
	}
}
