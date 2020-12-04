using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float coolDown = 0.5f;


	public Material realEffect;
	public Material digitalEffect;
	ScannerEffect scanner;

    public GameObject triggerDetector;
    bool isInWall;

	GameManager game;

	private void Start()
	{
		game = FindObjectOfType<GameManager>();

        triggerDetector.layer = 0;

		scanner = GetComponentInChildren<ScannerEffect>();

	}


	float endTime = 0;
	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isInWall && Time.time >= endTime)
        {
			game.ToggleDimension();

			endTime = Time.time + coolDown;
			if (GameManager.currentActiveDimension == Dimension.Real)
			{

				scanner.EffectMaterial = realEffect;
				scanner.Scan();
			}
			else if (GameManager.currentActiveDimension == Dimension.Digital)
			{
				scanner.EffectMaterial = digitalEffect;
				scanner.Scan();
            }
        }
    }



	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Projectile")) return;
        isInWall = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Projectile")) return;
        isInWall = false;
	}
}
