using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeycardIcon : MonoBehaviour
{

    public RectTransform displayPanel;
	public RectTransform backPanel;

	public Sprite keySprite;
	public Sprite noKeySprite;
	
	GameObject[] keyCards;
	Image[] imgs;

	private void Start()
	{
		Key[] _keycards = FindObjectsOfType<Key>();
		keyCards = new GameObject[_keycards.Length];
		imgs = new Image[_keycards.Length];

		backPanel.sizeDelta = new Vector2((_keycards.Length - 1) * 75, 0);
		
		for (int i = 0; i < _keycards.Length; i++)
		{
			GameObject key = _keycards[i].gameObject;
			keyCards[i] = key;

			RectTransform icon = CreateRect(key.transform.name, displayPanel);

			// Add the sprite icon
			imgs[i] = icon.gameObject.AddComponent<Image>();
			imgs[i].sprite = noKeySprite;
			imgs[i].color = key.GetComponentInChildren<Renderer>().material.color;

			// Position the icon
			icon.anchorMin = new Vector2(0, 0);
			icon.anchorMax = new Vector2(1, 1);

			icon.sizeDelta = new Vector2(0, 0);
			icon.anchoredPosition = new Vector2(-i * 75, 0);
		}
	}


	// Update is called once per frame
	void Update()
    {
        
    }

	public void KeyCollected(GameObject key)
	{
		for (int i = 0; i < keyCards.Length; i++)
		{
			if (key == keyCards[i])
			{
				imgs[i].sprite = keySprite;
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
