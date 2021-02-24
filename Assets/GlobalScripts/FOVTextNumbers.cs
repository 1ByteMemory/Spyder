using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FOVTextNumbers : MonoBehaviour
{
	public TextMeshProUGUI text;
	public Slider slider;
	public bool roundDown;

	private void Update()
	{
		if (roundDown)
		{

			text.text = (Mathf.Round(slider.value * 10) / 10).ToString();
		}
		else
		{
			text.text = slider.value.ToString();
		}
	}

}
