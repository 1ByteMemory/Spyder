using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniqueID : MonoBehaviour
{
	public int id;
	public int ID
	{
		get { return id; }
		private set { id = value; }
	}

	private void Awake()
	{
		float id = (1000 * transform.position.x) + transform.position.y + (0.001f * transform.position.z);
		ID = Mathf.RoundToInt(id * 1000);
	}
}
