using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wave : MonoBehaviour
{
    public GameObject[] EnemyList;
    public EnemyWaveSpawner[] SpawnerList;

    public UnityEvent OnWaveComplete;
	int numCompleted;
	
	private void Update()
	{
		numCompleted = 0;
		for (int i = 0; i < SpawnerList.Length; i++)
		{
			if (SpawnerList[i].IsWaveCompleted)
			{
				numCompleted++;
			}
		}



		if(numCompleted == SpawnerList.Length)
		{
			OnWaveComplete.Invoke();
		}
	}
}
