using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
	public float moveAmount = 5;

	public Transform model;

	public Vector3 openPos;
	Vector3 startPos;

	public AudioClip openSound;
	private AudioSource src;

	private void Start()
	{
		startPos = model.localPosition;
		src = GetComponent<AudioSource>();
		if (src == null)
		{
			src = gameObject.AddComponent <AudioSource>();
			src.playOnAwake = false;
		}
		src.clip = openSound;
	}

	public void SetDoor(bool open)
	{
		if (open)
		{
			src.Play();
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
