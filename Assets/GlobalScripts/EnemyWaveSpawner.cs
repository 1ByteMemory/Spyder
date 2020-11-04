﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{

	public GameObject enemy;

    public float spawnRate;
    public int spawnAmount;

	float spawnEndTime;
	
    public int remainingEnemies;
	public int spawnedEnemies;

	bool spawnAnother = true;
	bool spawningWave;

	bool delaying;

	private void Start()
	{
		SpawnEnemy();
	}

	private void Update()
	{
		if (spawnAnother)
		{
			if (!delaying)
			{
				delaying = true;
				spawnEndTime = Time.time + spawnRate;
			}

			if (Time.time >= spawnEndTime)
			{
				delaying = false;
				SpawnEnemy();
			}
		}
	}

	void SpawnEnemy()
	{
		if (spawnedEnemies <= spawnAmount)
		{
			spawnedEnemies++;
			remainingEnemies++;
			Instantiate(enemy, transform);
		}
	}


	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			spawnAnother = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			spawnAnother = true;
			delaying = false;
		}
	}


}