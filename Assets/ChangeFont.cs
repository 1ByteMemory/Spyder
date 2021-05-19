using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeFont : MonoBehaviour
{
    public bool changeFont;
    public TMP_FontAsset font;

	private void OnValidate()
	{
		if (changeFont)
		{
			changeFont = false;
			TextMeshProUGUI[] text = GetComponentsInChildren<TextMeshProUGUI>();
			Debug.Log(text.Length);

			for (int i = 0; i < text.Length; i++)
			{
				if (!text[i].GetComponent<AccessableColors>())
				{

					AccessableColors accss = text[i].gameObject.AddComponent<AccessableColors>();
					accss.colorType = ColorType.TextColors;
					//text[i].font = font;
				}
			}
		}
	}

}
