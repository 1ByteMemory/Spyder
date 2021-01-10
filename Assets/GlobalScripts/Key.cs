using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	[HideInInspector]
	public bool isHeld = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isHeld = true;
			gameObject.SetActive(false);
		}
	}
}
