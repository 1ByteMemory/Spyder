﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnounceStopStartLoop : MonoBehaviour
{

    public AudioClip Announce2;
    private AudioSource Announcesource2;
    //public AudioSource combatsource;


    void Start()
    {
        Announcesource2 = GameObject.Find("Announce").GetComponent<AudioSource>();
        //GetComponent<AudioSource> ().playOnAwake.false;
        //GetComponent<AudioSource> ().clip = combat
    }



    // Start is called before the first frame update
    void OnTriggerEnter()
    {
        Announcesource2.clip = Announce2;
        Announcesource2.Play();
        Announcesource2.loop = true;
        gameObject.SetActive(false);
        //combatsource.clip = combat;
        //combatsource.Play();
    }

    // Update is called once per frame
    public void OnTriggerExit()
    {
        //combatsource.clip = combat;
        //combatsource.Play();
        //gameObject.SetActive(false);
        //GetComponent<AudioSource>().Play();
    }
}
