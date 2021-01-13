using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
	public Wave wave;
	bool hasWaveStarted = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !hasWaveStarted && wave != null)
		{
			hasWaveStarted = true;
			wave.SpawnEnemies();
		}
	}
}
