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

	public CPSlider R, G, B;

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
		colorPanel.SetActive(false);

		OnColorPick.Invoke();
	}

	public void CancelChanges()
	{
		img.color = oldColor;

		R.SetValue(oldColor.r);
		G.SetValue(oldColor.g);
		B.SetValue(oldColor.b);

		OnColorPick.Invoke();
	}

	public void SetColour(Color col)
	{
		col.a = 1;
		img.color = col;

		R.SetValue(col.r);
		G.SetValue(col.g);
		B.SetValue(col.b);
	}


	public void ChangeColour()
	{
		float r = R != null ? R.value : 1;
		float g = G != null ? G.value : 1;
		float b = B != null ? B.value : 1;

		color = new Color(r, g, b, 1);
		img.color = new Color(r, g, b, 1);
	}
}
