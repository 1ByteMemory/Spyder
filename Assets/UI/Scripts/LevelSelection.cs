using System.Collections;
using System.Collections.Generic;
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

    public LevelInfo[] showCaseLevels;

	private List<LevelInfo> quickSaves = new List<LevelInfo>();
	private List<LevelInfo> autoSaves = new List<LevelInfo>();

	public void NewSave(LevelInfo save, SaveType saveType)
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

	public static void DeleteSave(LevelInfo save, SaveType saveType)
	{
		switch (saveType)
		{
			case SaveType.Quick:
				break;
			case SaveType.Auto:
				break;
		}
	}

	private void Start()
	{

		RectTransform prevRect = new RectTransform();
		sceneLoader = GetComponent<SceneLoader>();

		// How many cards can fit horizontally
		CanvasScaler canvas = GetComponentInParent<CanvasScaler>();
		Vector2 canvasSize = canvas.referenceResolution;

		RectTransform scrollRect = showCaseView.GetComponent<RectTransform>();
		showCaseView.content.sizeDelta = new Vector2(0, showCaseLevels.Length * textHieght);

		CreateList(showCaseView, showCaseLevels);
		//CreateList(showCaseView, autoSaves.ToArray());
	}
	

	public void CreateList(ScrollRect scrollRect, LevelInfo[] levelList)
	{
		for (int i = 0; i < levelList.Length; i++)
		{
			// Destroy current button to create a new one
			// Maybe not the best solution?
			Destroy(scrollRect.content.GetChild(i));

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
			button.onClick.AddListener(() => LoadScene(i));

		}

	}

	public void LoadScene(int index)
	{
		GameManager.loadedFromSelector = true;
		GameManager.loadedSpawnPosition = showCaseLevels[index].spawnPoint;
		GameManager.loadedWeapons = showCaseLevels[index].availableWeapons;

		sceneLoader.LoadScene(showCaseLevels[index].sceneName);
	}

	private RectTransform CreateRect(string name, RectTransform parent)
	{
		RectTransform rect = new GameObject().AddComponent<RectTransform>();
		rect.name = name;
		rect.SetParent(parent, false);
		return rect;
	}
}
