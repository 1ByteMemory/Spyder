using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wave : MonoBehaviour
{
    public GameObject[] EnemyList;
    public EnemyWaveSpawner[] SpawnerList;

    public UnityEvent OnWaveComplete;
	public int numCompleted;
	public int maxEnemies;

	private void Start()
	{
		foreach (EnemyWaveSpawner item in SpawnerList)
		{
			maxEnemies += item.spawnAmount;
		}

		maxEnemies += EnemyList.Length;

	}

	private void Update()
	{
		numCompleted = 0;

		for (int i = 0; i < EnemyList.Length; i++)
		{
			if (EnemyList[i].gameObject == null)
			{
				numCompleted++;
			}
		}


		for (int i = 0; i < SpawnerList.Length; i++)
		{
			if (SpawnerList[i].IsWaveCompleted)
			{
				numCompleted++;
			}
		}

		if(numCompleted == maxEnemies)
		{
			OnWaveComplete.Invoke();
		}
	}
}
