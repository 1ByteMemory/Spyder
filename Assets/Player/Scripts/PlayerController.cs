using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    int currentDimension;
    public Camera camA;
    public Camera camB;

    public GameObject triggerDetector;
    bool isInWall;

    public int universeLayer1;
    public int universeLayer2;


	private void Start()
	{
        triggerDetector.layer = 0;
        IgnoreLayer(universeLayer2, universeLayer1);
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
			}
			else
			{
                // Switch to universe B
                currentDimension = 1;
                IgnoreLayer(universeLayer1, universeLayer2);
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
