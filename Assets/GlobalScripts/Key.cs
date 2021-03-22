using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	[HideInInspector]
	public bool isHeld = false;

	public AudioClip pickUpSound;
	private Transform src;


	private void Start()
	{
		src = transform.GetChild(2);

		src.GetComponent<AudioSource>().clip = pickUpSound;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			src.parent = null;
			src.GetComponent<AudioSource>().Play();
			isHeld = true;
			gameObject.SetActive(false);
		}
	}
}
