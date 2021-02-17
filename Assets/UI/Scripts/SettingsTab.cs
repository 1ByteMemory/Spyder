using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsTab : MonoBehaviour
{
	public Color OpenColor;

	private Color defaultColor;

	public GameObject[] Menus;
	public GameObject[] TabsLabels;

	private void Start()
	{
		defaultColor = TabsLabels[0].GetComponent<Image>().color;
	}

	public void OpenTab(GameObject tab)
	{
		foreach (GameObject obj in Menus)
		{
			obj.SetActive(false);
		}

		tab.SetActive(true);
	}


	public void TabSelected(Image img)
	{
		foreach (GameObject obj in TabsLabels)
		{
			obj.GetComponent<Image>().color = defaultColor;
		}

		img.color = OpenColor;
	}

}
