using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
	public LevelInfo[] quickSaves;
	public LevelInfo[] checkpoints;
	LevelSelection selection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
		if (Input.GetKey(KeyCode.F5))
		{
			// Quick Save

			

		}

		if (Input.GetKey(KeyCode.F6))
		{
			// Quick Load


		}
    }
}
