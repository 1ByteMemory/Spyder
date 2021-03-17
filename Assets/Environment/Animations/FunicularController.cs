﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunicularController : MonoBehaviour
{

	Animator anim;
	public AudioClip fhydro;
	AudioSource fhydrosource;
	//public AudioClip bgmrock;
	//AudioSource fsoundsiren;



	private void Start()
	{
		anim = GetComponent<Animator>();
		fhydrosource = GetComponent<AudioSource>();
		//fsoundsiren = GetComponent<AudioSource>();
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
		fhydrosource.clip = fhydro;
		fhydrosource.Play();
	}
	public void Stop()
	{
		fhydrosource.Stop();
		//bgm.clip = bgmrock;
		//bgm.Play();

	}
	public void StopAnim()
	{
		anim.speed = 0;
		
	}

}
