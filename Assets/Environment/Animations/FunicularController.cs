using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunicularController : MonoBehaviour
{

	Animator anim;


	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void PlayAnim()
	{
		anim.SetBool("Play", true);
	}


	// These methods are called from the animator event keys
	public void Go()
	{
		GameObject.FindGameObjectWithTag("Player").transform.parent = transform;
	}
	public void Stop()
	{
		GameObject.FindGameObjectWithTag("Player").transform.parent = null;
	}
	public void StopAnim()
	{
		anim.speed = 0;
	}

	
}
