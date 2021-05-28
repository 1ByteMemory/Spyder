using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatAudio
{
	[Range(0, 1)]
	public float volume = 1;
    public Material material;
	public AudioClip audioClip;
}


[RequireComponent(typeof(AudioSource))]
public class FootstepsManager : MonoBehaviour
{
	public float stride;

	public AudioClip defaultAudio;
    public MatAudio[] pairs;

	private PlayerMovement playerMove;
	private AudioSource src;
	private float strideEndTime;

	public static float volume;

	private void Start()
	{
		playerMove = GetComponent<PlayerMovement>();
		src = GetComponents<AudioSource>()[0];
	}

	private void Update()
	{
		if (playerMove.IsInAir)
		{
			strideEndTime = 0;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.collider.CompareTag("Untagged") && playerMove.IsMoving())
		{
			float direction = hit.point.y - transform.position.y;

			if (direction < 0)
			{
				// Play foot steps

				if (Time.time >= strideEndTime)
				{
					strideEndTime = Time.time + stride;

					Renderer ren = hit.gameObject.GetComponent<Renderer>();
					MatAudio matAudio = ren != null ? GetAudio(ren.sharedMaterial) : null;
					
					if (ren != null && matAudio != null)
					{
						src.clip =  matAudio.audioClip;
						src.volume = matAudio.volume * volume;
					}
					else
					{
						src.clip =  defaultAudio;
						src.volume = volume;
					}
					src.Play();
				}
			}
		}
	}

	public MatAudio GetAudio(Material material)
	{
		MatAudio matAudio = null;
		for (int i = 0; i < pairs.Length; i++)
		{
			if (pairs[i].material == material)
			{
				matAudio = pairs[i];
			}
		}
		return matAudio;
	}
}