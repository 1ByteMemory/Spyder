﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float coolDown = 0.5f;

	public Material digitalEffect;
	ScannerEffect scanner;

    public GameObject triggerDetector;
    bool isInWall;

	GameManager game;

	private void Start()
	{
		game = FindObjectOfType<GameManager>();

        triggerDetector.layer = 0;

		scanner = GetComponentInChildren<ScannerEffect>();

	}


	float endTime = 0;
	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isInWall && Time.time >= endTime)
        {
			game.ToggleDimension();

			endTime = Time.time + coolDown;
			scanner.Scan(GameManager.currentActiveDimension);
        }
    }

	private void OnDisable()
	{
		digitalEffect.SetFloat("_ScanDistance", 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Projectile")) return;
		if (other.isTrigger) return;
        isInWall = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Projectile")) return;
		if (other.isTrigger) return;
        isInWall = false;
	}
}
