using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundChecker
{
    public static LayerMask layerMask;
    public static bool IsGrounded(Ray ray, float distance)
	{
        return Physics.Raycast(ray, distance, layerMask, QueryTriggerInteraction.Ignore);
	}
}
