using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void SetColor(Color col);
public enum ColorType
{
	DigitalColors,
	TextColors
}

public class AccessableColors : MonoBehaviour
{
	public static Color digitalColor;
	public static Color textColor;
	public ColorType colorType;
	public Image imgCol;
    public TextMeshPro textCol;
    public TextMeshProUGUI textColUGUI;
	
	public static SetColor setColDelagate;
	public static SetColor setTextColor;

	private bool componantAdded;

	private void OnValidate()
	{
		if (!componantAdded)
		{
			componantAdded = true;
			if (GetComponent<Image>())
			{
				imgCol = GetComponent<Image>();
			}
			if (GetComponent<TextMeshPro>())
			{
				textCol = GetComponent<TextMeshPro>();
			}
			if (GetComponent<TextMeshProUGUI>())
			{
				textColUGUI = GetComponent<TextMeshProUGUI>();
			}
		}
	}

	private void Start()
	{
		switch (colorType)
		{
			case ColorType.DigitalColors:
				setColDelagate += ChangeCol;
				break;
			case ColorType.TextColors:
				setTextColor += ChangeCol;
				break;
			default:
				break;
		}
		setColDelagate?.Invoke(digitalColor);
		setTextColor?.Invoke(textColor);
	}
	private void OnDisable()
	{
		if (imgCol != null)
			setColDelagate -= ChangeCol;
	}


	private void ChangeCol(Color col)
	{
        if (imgCol != null)
		{
            imgCol.color = col;
		}
		if (textColUGUI != null)
		{
			textColUGUI.color = col;
		}
		if (textCol != null)
		{
			textCol.color = col;
		}
	}
}
