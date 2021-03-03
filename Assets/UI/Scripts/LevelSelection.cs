using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public ScrollRect scroll;

	public Vector2 border;
	public Vector2 cardGap;

    public LevelInfo[] levels;
	
	private void Start()
	{
		RectTransform prevRect = new RectTransform();
		int numInRow = Mathf.RoundToInt(scroll.content.sizeDelta.x / (128 + cardGap.x));

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
				card.anchoredPosition = new Vector2(64 + border.x, -64 - border.y);
				prevRect = card;
			}
			else
			{
				float x;
				float y;
				if (i % numInRow == 0)
				{
					
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
