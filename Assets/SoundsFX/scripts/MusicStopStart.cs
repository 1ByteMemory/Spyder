﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStopStart : MonoBehaviour
{

    public AudioClip combat;
    private AudioSource combatsource;
    //public AudioSource combatsource;
   

    void Start()
    { 
        combatsource = GameObject.Find("Music").GetComponent<AudioSource>();
        //GetComponent<AudioSource> ().playOnAwake.false;
        //GetComponent<AudioSource> ().clip = combat
    }



    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatsource.clip = combat;
            combatsource.Play();
            gameObject.SetActive(false);
        }
        //combatsource.clip = combat;
        //combatsource.Play();
    }

    // Update is called once per frame
    public void OnTriggerExit ()
    {
        //combatsource.clip = combat;
        //combatsource.Play();
        //gameObject.SetActive(false);
        //GetComponent<AudioSource>().Play();
    }
}
