﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        collision.GetComponent<Collider>().transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider collision)
    {
        collision.GetComponent<Collider>().transform.SetParent(null);
    }
}