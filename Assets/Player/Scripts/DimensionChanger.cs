using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DimensionChanger : MonoBehaviour
{
	public Transform[] spawnPoints;

	public Transform GetDimension(int dimensionNumber)
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			if (i == dimensionNumber)
			{
				return spawnPoints[i];
			}
		}
		Debug.LogError("DimensionNumber Out of Bounds!");
		return null;
	}
}
