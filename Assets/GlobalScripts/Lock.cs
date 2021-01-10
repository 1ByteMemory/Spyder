using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public Key key;
	public DoorController[] doors;

	public Material activatedMat;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && key.isHeld)
		{
			foreach (DoorController door in doors)
			{
				door.OpenDoor();

				if (activatedMat != null) transform.GetChild(1).GetComponent<MeshRenderer>().material = activatedMat;
			}
		}
	}
}
