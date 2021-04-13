using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	[HideInInspector]
	public bool isHeld = false;

	public AudioClip pickUpSound;
	private Transform src;
	private GameManager game;

	private void Start()
	{
		game = FindObjectOfType<GameManager>();

		src = transform.GetChild(2);

		src.GetComponent<AudioSource>().clip = pickUpSound;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			game.PlayerHUD.GetComponent<KeycardIcon>().KeyCollected(gameObject);

			src.parent = null;
			src.GetComponent<AudioSource>().Play();
			isHeld = true;
			gameObject.SetActive(false);
		}
	}

	public void Loaded()
	{
		src.parent = null;
		isHeld = true;
		gameObject.SetActive(false);
	}
}
