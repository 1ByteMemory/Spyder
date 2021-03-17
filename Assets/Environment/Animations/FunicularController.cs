using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunicularController : MonoBehaviour
{

	Animator anim;
	public AudioClip fhydro;
	public AudioClip bgmrock;
	AudioSource bgm;
	AudioSource fsoundsiren;



	private void Start()
	{
		anim = GetComponent<Animator>();
		bgm = GetComponent<AudioSource>();
		fsoundsiren = GetComponent<AudioSource>();
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
		fsoundsiren.clip = fhydro;
		fsoundsiren.Play();
	}
	public void Stop()
	{
		fsoundsiren.Stop();
		bgm.clip = bgmrock;
		bgm.Play();

	}
	public void StopAnim()
	{
		anim.speed = 0;
		
	}

}
