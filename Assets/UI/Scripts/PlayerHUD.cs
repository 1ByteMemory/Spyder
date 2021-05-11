using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public RectTransform displayPanel;
	public TextMeshProUGUI AmmoClip;
	public TextMeshProUGUI AmmoReserve;
	public TextMeshProUGUI health;
	
	//public RectTransform backPanel;

	public Sprite keySprite;
	public Sprite noKeySprite;
	
	GameObject[] keyCards;
	Image[] imgs;

	public static bool[] keysFound;

	private void Start()
	{
		Key[] _keycards = FindObjectsOfType<Key>();
		keyCards = new GameObject[_keycards.Length];
		imgs = new Image[_keycards.Length];

		float width = displayPanel.rect.width;
		float height = displayPanel.rect.height;

		displayPanel.sizeDelta = new Vector2(_keycards.Length * width, height);
		
		for (int i = 0; i < _keycards.Length; i++)
		{
			GameObject key = _keycards[i].gameObject;
			keyCards[i] = key;

			RectTransform icon = CreateRect(key.transform.name, displayPanel);

			// Add the sprite icon
			imgs[i] = icon.gameObject.AddComponent<Image>();
			imgs[i].sprite = noKeySprite;
			//imgs[i].color = key.GetComponentInChildren<Renderer>().material.color;

			// Position the icon
			icon.anchorMin = new Vector2(0, 0);
			icon.anchorMax = new Vector2(0, 1);

			icon.sizeDelta = new Vector2(width, 0);
			icon.anchoredPosition = new Vector2(i * width + (width / 2), 0);
		}

		if (keysFound == null)
			keysFound = new bool[_keycards.Length];
		
		for (int i = 0; i < keysFound.Length; i++)
		{
			if (keysFound[i])
			{
				KeyCollected(keyCards[i]);
				keyCards[i].GetComponent<Key>().Loaded();
			}
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
				keysFound[i] = true;
				imgs[i].sprite = keySprite;
				imgs[i].color = key.GetComponentInChildren<Renderer>().material.color;
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
