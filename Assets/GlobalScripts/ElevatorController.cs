using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
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
	public void SetPlayerChild()
	{
		GameObject.FindGameObjectWithTag("Player").transform.parent = transform;
	}
	public void SetPlayerParentNull()
	{
		GameObject.FindGameObjectWithTag("Player").transform.parent = null;
	}
	public void StopAnim()
	{
		anim.speed = 0;
	}

}
