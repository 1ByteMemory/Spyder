using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[ExecuteInEditMode]
public class CPButton : MonoBehaviour
{

    public Image img;
	public GameObject colorPanel;

	//public CPSlider R, G, B;
	public FlexibleColorPicker colorPicker;

	public Color color;

	private Color oldColor;

	private RectTransform rect;

	public UnityEvent OnColorPick;

	private void Start()
	{
		rect = GetComponent<RectTransform>();
	}

	private void OnEnable()
	{
		SetColour(color);
	}

	private void OnValidate()
	{
		SetColour(color);
	}


	public void ToggleColorPicker()
	{
		// Close other color pickers that might be open
		GameObject[] colorPickers = GameObject.FindGameObjectsWithTag("ColorPicker");
		for (int i = 0; i < colorPickers.Length; i++)
		{
			colorPickers[i].SetActive(false);
		}

		// Check if button is clicked and not the colour display panel
		if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
		{
			colorPanel.SetActive(!colorPanel.activeSelf);

			if (colorPanel.activeSelf)
			{
				oldColor = img.color;
			}
		}
	}

	public void OpenColorPicker()
	{
		oldColor = img.color;
		colorPanel.SetActive(true);
	}

	public void CloseColorPicker()
	{
		ChangeColour();
		//OnColorPick.Invoke();
		colorPanel.SetActive(false);
	}

	public void CancelChanges()
	{
		img.color = oldColor;

		colorPicker.color = oldColor;
		OnColorPick.Invoke();
	}

	public void SetColour(Color col)
	{
		col.a = 1;
		img.color = col;

		colorPicker.color = col;
	}


	public void ChangeColour()
	{
		color = colorPicker.color;
		img.color = colorPicker.color;
		OnColorPick.Invoke();
	}
}
