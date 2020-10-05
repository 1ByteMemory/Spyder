using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DimensionChanger
{
	public Camera[] cams;

	public void ChangeDimension(int dimensionNumber)
	{
		for (int i = 0; i < cams.Length; i++)
		{
			cams[i].depth = i == dimensionNumber ? 1 : 0;
		}
	}
}
