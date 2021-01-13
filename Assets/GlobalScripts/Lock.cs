using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour
{
    public Key key;
	public UnityEvent OnUnlock;

	public Material activatedMat;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && key.isHeld)
		{
			OnUnlock.Invoke();

			if (activatedMat != null) transform.GetChild(1).GetComponent<MeshRenderer>().material = activatedMat;
		}
	}
}
