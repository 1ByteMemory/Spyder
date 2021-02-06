using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    public Key key;
	public UnityEvent OnUnlock;

	public Material activatedMat;

	public AudioClip accessGranted;
	public AudioClip accessDenied;

	private AudioSource src;
	private bool hasActivated;

	private void Start()
	{
		src = GetComponent<AudioSource>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (key.isHeld)
			{
				if (!hasActivated)
				{
					if (activatedMat != null) transform.GetChild(1).GetComponent<MeshRenderer>().material = activatedMat;

					hasActivated = true;
					src.clip = accessGranted;
					src.Play();
					OnUnlock.Invoke();
				}
			}
			else
			{
				src.clip = accessDenied;
				src.Play();
			}
		}
	}
}
