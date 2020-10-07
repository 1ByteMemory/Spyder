using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : DimensionChanger
{

    int currentDimension;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentDimension != 0)
        {
            currentDimension = 0;

            if (GetDimension(0) == null) return;
            transform.position = GetDimension(0).position;
            transform.rotation = GetDimension(0).rotation;
        }
        if (Input.GetMouseButtonDown(1) && currentDimension != 1)
        {
            currentDimension = 1;
            
            if (GetDimension(1) == null) return;
            transform.position = GetDimension(1).position;
            transform.rotation = GetDimension(1).rotation;
        }
    }
}
