using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static int currentDimension;

	public float coolDown = 0.5f;

    public Material materialA;
    public Material materialB;

	public Material realEffect;
	public Material digitalEffect;
	SannerEffect scanner;

    public GameObject triggerDetector;
    bool isInWall;

    public int universeLayer1;
    public int universeLayer2;

	static int layerA;
	static int layerB;

	public static int GetActiveLayer()
	{
		return currentDimension == 0 ? layerA : layerB;
	}

	

	private void Start()
	{
		layerA = universeLayer1;
		layerB = universeLayer2;

        triggerDetector.layer = 0;
        IgnoreLayer(universeLayer2, universeLayer1);

		// <<<< TEMPORAY - DO NOT KEEP >>>> //
		Color col = materialA.GetColor("_Color");
		col.a = 1f;

		materialA.SetColor("_Color", col);

		col = materialB.GetColor("_Color");
		col.a = 0.25f;

		materialB.SetColor("_Color", col);
		// <<<<<<<<<<<<<<     >>>>>>>>>>> //

		scanner = GetComponentInChildren<SannerEffect>();

	}


	float endTime = 0;
	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isInWall && Time.time >= endTime)
        {
			endTime = Time.time + coolDown;
			if (currentDimension == 1)
			{
				// Switch to universe A
				currentDimension = 0;
				IgnoreLayer(universeLayer2, universeLayer1);

				// <<<< TEMPORAY - DO NOT KEEP >>>> //
				Color col = materialA.GetColor("_Color");
				col.a = 1f;

				materialA.SetColor("_Color", col);

				col = materialB.GetColor("_Color");
				col.a = 0.25f;

				materialB.SetColor("_Color", col);
				// <<<<<<<<<<<<<<     >>>>>>>>>>> //

				scanner.EffectMaterial = realEffect;
				scanner.Scan();
			}
			else
			{
				// Switch to universe B
				currentDimension = 1;
				IgnoreLayer(universeLayer1, universeLayer2);


                // <<<< TEMPORAY - DO NOT KEEP >>>> //
                Color col = materialA.GetColor("_Color");
                col.a = 0.25f;

                materialA.SetColor("_Color", col);

                col = materialB.GetColor("_Color");
                col.a = 1f;

                materialB.SetColor("_Color", col);
				// <<<<<<<<<<<<<<     >>>>>>>>>>> //

				scanner.EffectMaterial = digitalEffect;
				scanner.Scan();
            }
        }
    }


    void IgnoreLayer(int ignoreLayer, int collideLayer)
    {
        // 10 is the Player's layer
        Physics.IgnoreLayerCollision(10, ignoreLayer, true); // true - do ignore these collisions
        Physics.IgnoreLayerCollision(10, collideLayer, false); // false - do not ignore these collisions....confusing I know
    }


	public static void IgnoreLayer(int targetLayer, int ignoreLayer, int collideLayer)
	{
		Physics.IgnoreLayerCollision(targetLayer, ignoreLayer, true);
		Physics.IgnoreLayerCollision(targetLayer, collideLayer, false);
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
