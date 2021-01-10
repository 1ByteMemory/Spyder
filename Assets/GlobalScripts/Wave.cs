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
	int maxEnemies;

	private void Start()
	{
		foreach (GameObject item in EnemyList)
		{
			item.SetActive(false);
		}

		foreach (EnemyWaveSpawner item in SpawnerList)
		{
			maxEnemies += item.spawnAmount;
			item.gameObject.SetActive(false);
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

	public void SpawnEnemies()
	{
		foreach (GameObject item in EnemyList)
		{
			item.SetActive(true);
		}

		foreach (EnemyWaveSpawner item in SpawnerList)
		{
			item.gameObject.SetActive(true);
		}
	}
}
