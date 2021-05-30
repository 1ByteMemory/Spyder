using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControl : MonoBehaviour
{

    [SerializeField] private Animator animationcontroller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animationcontroller.SetBool("Playanimation", true);
        }
    }

    
}
