using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform PointA;



	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.transform.position = PointA.position;
		}
	}

}
