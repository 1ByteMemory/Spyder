using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    int currentDimension;
    public Camera camA;
    public Camera camB;

    public Material materialA;
    public Material materialB;


    public GameObject triggerDetector;
    bool isInWall;

    public int universeLayer1;
    public int universeLayer2;


	private void Start()
	{
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
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isInWall)
        {
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
            }
        }
    }


    void IgnoreLayer(int ignoreLayer, int collideLayer)
    {
        // 10 is the Player's layer
        Physics.IgnoreLayerCollision(10, ignoreLayer, true); // true - do ignore these collisions
        Physics.IgnoreLayerCollision(10, collideLayer, false); // false - do not ignore these collisions....confusing I know
    }

	private void OnTriggerEnter(Collider other)
	{
        isInWall = true;
	}

	private void OnTriggerExit(Collider other)
	{
        isInWall = false;
	}
}
