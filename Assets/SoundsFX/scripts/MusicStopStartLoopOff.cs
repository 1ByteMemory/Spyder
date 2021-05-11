using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStopStartLoopOff : MonoBehaviour
{

    public AudioClip combat3;
    private AudioSource combatsource3;
    //public AudioSource combatsource;


    void Start()
    {
        combatsource3 = GameObject.Find("Music").GetComponent<AudioSource>();
        //GetComponent<AudioSource> ().playOnAwake.false;
        //GetComponent<AudioSource> ().clip = combat
    }



    // Start is called before the first frame update
    void OnTriggerEnter()
    {
        combatsource3.clip = combat3;
        combatsource3.Play();
        combatsource3.loop = false;
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
