using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Colour
{
	Red,
	Green,
	Blue
}

[RequireComponent(typeof(Slider))]
[ExecuteInEditMode]
public class CPSlider : MonoBehaviour
{
	public Colour colour;

	[HideInInspector]
	public float value;

	public TextMeshProUGUI text;

	private Slider slider;

	public void OnEnable()
	{
		slider = GetComponent<Slider>();

		if (text != null)
		{
			text.text = Mathf.FloorToInt(slider.value * 255).ToString();
		}
		value = slider.value;
	}

	public void SetValue(float value)
	{
		this.value = value;

		if (text != null)
		{
			// Convert value to 0-255
			int bit = Mathf.FloorToInt(value * 255);

			text.text = bit.ToString();
		}


		// If the value is being changed else where
		if (slider != null && value != slider.value)
		{
			slider.value = value;
		}
	}

}
