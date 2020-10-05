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
            transform.position = GetDimension(0).position;
            transform.rotation = GetDimension(0).rotation;
        }
        if (Input.GetMouseButtonDown(1) && currentDimension != 1)
        {
            currentDimension = 1;
            transform.position = GetDimension(1).position;
            transform.rotation = GetDimension(1).rotation;
        }
    }
}
