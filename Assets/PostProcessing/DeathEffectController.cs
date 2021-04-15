using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public delegate void PlayerDeathEvent();

public class DeathEffectController : MonoBehaviour
{

    PostProcessVolume volume;
    static bool isPlaying;
	public float speed;

	public event PlayerDeathEvent PlayerDeath;

	private void Start()
	{
		volume = GetComponent<PostProcessVolume>();
		volume.weight = 0;
	}

	// Update is called once per frame
	void Update()
    {
        if (isPlaying)
		{
			if (volume.weight >= 1)
			{
				isPlaying = false;
				
				OnPlayerDeath();
			}
			volume.weight += speed * Time.deltaTime;

			Time.timeScale = Mathf.Clamp(1 - volume.weight, 0.3f, 1);
		}
    }

	protected virtual void OnPlayerDeath()
	{
		// if PlayerDeath is not null then call delegate
		PlayerDeath?.Invoke();
	}


    public void Play()
	{
        isPlaying = true;
	}
}
