using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float coolDown = 0.5f;

	public Material digitalEffect;
	ScannerEffect scanner;

    public GameObject triggerDetector;
    bool isInWall;

	GameManager game;

	CharacterController cc;

	public static bool toggleCrouch;
	public bool isAbilityUnlocked = false;

	private void Start()
	{
		game = FindObjectOfType<GameManager>();

        triggerDetector.layer = 0;

		scanner = GetComponentInChildren<ScannerEffect>();
		cc = GetComponent<CharacterController>();
	}


	float endTime = 0;
	// Update is called once per frame
	void Update()
    {
        if (isAbilityUnlocked && Input.GetMouseButtonDown(1) && !isInWall && Time.time >= endTime)
        {
			game.ToggleDimension();

			endTime = Time.time + coolDown;
			scanner.Scan(GameManager.currentDimension);
        }

		if (toggleCrouch)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				Crouch(!IsCrouching);
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				Crouch(true);
			}
			if (IsCrouching && !Input.GetKey(KeyCode.LeftShift))
			{
				Crouch(false);
			}
		}
	}

	private static bool _isCrouch = false;
	public static bool IsCrouching
	{
		get { return _isCrouch; }
	}

	void Crouch(bool isCrouch)
	{
		if (isCrouch)
		{
			_isCrouch = isCrouch;
			cc.height = 0.8f;
			//cc.center = new Vector3(0, 0.25f, 0);
		}
		else
		{
			if (!Physics.Raycast(transform.position, Vector3.up, out _, 1.5f))
			{
				_isCrouch = isCrouch;
				cc.height = 1.8f;
			}
		}
	}

	private void OnDisable()
	{
		digitalEffect.SetFloat("_ScanDistance", 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Projectile")) return;
		if (other.isTrigger) return;
        isInWall = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Projectile")) return;
		if (other.isTrigger) return;
        isInWall = false;
	}
}
