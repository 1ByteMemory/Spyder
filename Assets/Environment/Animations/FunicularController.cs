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

	void OnTriggerEnter(Collider collision)
	{
		collision.GetComponent<Collider>().transform.SetParent(transform);
	}

	void OnTriggerExit(Collider collision)
	{
		collision.GetComponent<Collider>().transform.SetParent(null);
	}

	// These methods are called from the animator event keys
	public void Go()
	{
		// GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().transform.SetParent(transform);
	}
	public void Stop()
	{
		// GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().transform.SetParent(null);
	}
	public void StopAnim()
	{
		anim.speed = 0;
	}

}
