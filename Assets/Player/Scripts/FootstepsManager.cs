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

	private void Start()
	{
		playerMove = GetComponent<PlayerMovement>();
		src = GetComponent<AudioSource>();
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

					MatAudio matAudio = GetAudio(hit.gameObject.GetComponent<Renderer>().sharedMaterial);

					src.clip = matAudio != null ? matAudio.audioClip : defaultAudio;
					src.volume = matAudio.volume;
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