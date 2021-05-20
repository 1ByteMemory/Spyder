using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : MonoBehaviour
{

	private void Start()
	{
		transform.GetChild(0).gameObject.SetActive(false);
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

			transform.GetChild(0).gameObject.SetActive(true);

			GameManager.IsGameWin = true;
		}
	}

}
