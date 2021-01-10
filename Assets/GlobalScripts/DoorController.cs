using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
	public float moveAmount = 5;

	public Transform model;

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


	public void OpenDoor()
	{
		SetDoor(true);
	}

}
