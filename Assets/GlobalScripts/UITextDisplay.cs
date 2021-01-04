using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void DisplayText(float value)
	{
		text.text = value.ToString();
	}
}
