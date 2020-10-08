using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : DimensionChanger
{

    int currentDimension;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            currentDimension = currentDimension == 0 ? 1 : 0;

            if (GetDimension(currentDimension) == null) return;
            transform.position = GetDimension(currentDimension).position;
            transform.rotation = GetDimension(currentDimension).rotation;
        }
    }
}
