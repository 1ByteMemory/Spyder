using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Activation
{
	Player,
	Enemy,
	WaveClear
}


public class DoorController : MonoBehaviour
{
	public float moveAmount = 5;

	public Transform model;

	public Activation activeOn;

	public Vector3 openPos;
	Vector3 startPos;

	private void Start()
	{
		startPos = model.localPosition;
	}

	public void SetDoor(bool open)
	{
		if (open)
		{
			model.localPosition = startPos + openPos;
		}
		else
		{
			model.localPosition = startPos;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (activeOn)
		{
			case Activation.Player:
				if (other.CompareTag("Player"))
				{
					SetDoor(true);
				}

				break;
			case Activation.Enemy:
				if (other.CompareTag("Enemy"))
				{
					SetDoor(true);
				}
				break;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		switch (activeOn)
		{
			case Activation.Player:
				if (other.CompareTag("Player"))
				{
					SetDoor(false);
				}

				break;
			case Activation.Enemy:
				if (other.CompareTag("Enemy"))
				{
					SetDoor(false);
				}
				break;
		}
	}


	public void WaveComplete()
	{
		SetDoor(true);
	}

}
