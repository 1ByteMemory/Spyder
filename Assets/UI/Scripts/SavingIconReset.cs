using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingIconReset : MonoBehaviour
{
    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

	private void OnDisable()
	{
        if (anim != null)
            anim.SetTrigger("Stop");
	}
}
