using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SceneLoader))]
public class LevelSelection : MonoBehaviour
{
    public ScrollRect scroll;
	SceneLoader sceneLoader;

	public Vector2 border;
	public Vector2 cardGap;

    public LevelInfo[] levels;
	
	private void Start()
	{
		RectTransform prevRect = new RectTransform();
		sceneLoader = GetComponent<SceneLoader>();

		// How many cards can fit horizontally
		CanvasScaler canvas = GetComponentInParent<CanvasScaler>();
		Vector2 canvasSize = canvas.referenceResolution;

		RectTransform scrollRect = scroll.GetComponent<RectTransform>();
		
		int Row = Mathf.RoundToInt((canvasSize.x - Mathf.Abs(scrollRect.sizeDelta.x)) / (128 + cardGap.x));
		scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, (178 + cardGap.y) * (levels.Length / Row) + border.y);


		for (int i = 0; i < levels.Length; i++)
		{
			// Create empty rect
			RectTransform card = CreateRect(levels[i].title, scroll.content);

			// Create Image
			RectTransform img = CreateRect("Image", card);
			img.gameObject.AddComponent<Image>().sprite = levels[i].image;

			// Create Title
			RectTransform title = CreateRect("Title", card);
			TextMeshProUGUI text = title.gameObject.AddComponent<TextMeshProUGUI>();
			text.text = levels[i].title;
			text.color = Color.black;
			text.fontSize = 21;
			text.alignment = TextAlignmentOptions.Top;


			// Set Sizes and position
			card.sizeDelta = new Vector2(128, 128);
			card.anchorMin = new Vector2(0, 1);
			card.anchorMax = new Vector2(0, 1);
			if (i == 0)
			{
				// First card is set
				card.anchoredPosition = new Vector2(64 + border.x, -64 - border.y);
				prevRect = card;
			}
			else
			{
				float x;
				float y;
				if (i % Row == 0)
				{
					// Place card on a new row 
					x = 64 + border.x;
					y = prevRect.anchoredPosition.y - 178 - cardGap.y;
				}
				else
				{
					x = prevRect.anchoredPosition.x + 128 + cardGap.x;
					y = prevRect.anchoredPosition.y;
				}

				card.anchoredPosition = new Vector2(x, y);

				prevRect = card;
			}

			img.anchorMin = Vector2.zero;
			img.anchorMax = Vector2.one;
		    img.anchoredPosition = Vector2.zero;
			img.sizeDelta = Vector2.zero;

			title.anchorMin = Vector2.zero;
			title.anchorMax = new Vector2(1, 0);
			title.anchoredPosition = new Vector2(0, -25);
			title.sizeDelta = new Vector2(0, 50);


			// Button
			Button button = card.gameObject.AddComponent<Button>();
			button.targetGraphic = img.GetComponent<Image>();
			
			// For some reason this is how you change button colors
			ColorBlock block = button.colors;
			block.highlightedColor = new Color(0.7f, 0.7f, 0.7f);
			button.colors = block;

			// Add LoadLevel Method to OnClick event
			int index = i;
			button.onClick.AddListener(() => LoadScene(index));

		}
	}

	public void LoadScene(int index)
	{
		GameManager.loadedFromSelector = true;
		GameManager.loadedSpawnPosition = levels[index].spawnPoint;
		GameManager.loadedWeapons = levels[index].availableWeapons;

		sceneLoader.LoadScene(levels[index].sceneName);
	}

	private RectTransform CreateRect(string name, RectTransform parent)
	{
		RectTransform rect = new GameObject().AddComponent<RectTransform>();
		rect.name = name;
		rect.SetParent(parent, false);
		return rect;
	}


}
