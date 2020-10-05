using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public DimensionChanger dimensionChanger;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dimensionChanger.ChangeDimension(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            dimensionChanger.ChangeDimension(1);
        }
    }
}
