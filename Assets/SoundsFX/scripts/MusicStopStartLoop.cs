using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStopStartLoop : MonoBehaviour
{

    public AudioClip combat2;
    private AudioSource combatsource2;
    //public AudioSource combatsource;


    void Start()
    {
        combatsource2 = GameObject.Find("Music").GetComponent<AudioSource>();
        //GetComponent<AudioSource> ().playOnAwake.false;
        //GetComponent<AudioSource> ().clip = combat
    }



    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            combatsource2.clip = combat2;
            combatsource2.Play();
            combatsource2.loop = true;
            gameObject.SetActive(false);
        }
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
