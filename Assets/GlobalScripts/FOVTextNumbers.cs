using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FOVTextNumbers : MonoBehaviour
{
	public TextMeshProUGUI text;
	public Slider slider;

	private void Update()
	{
		text.text = slider.value.ToString();
	}

}
