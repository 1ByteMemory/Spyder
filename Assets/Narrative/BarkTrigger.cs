using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusTrigger : MonoBehaviour
{
    //defines the flowchart and the block that flowchart contains
    public Fungus.Flowchart DialogFlow;
    public string targetBlock = "BarkTimerTrigger";

    //An onTrigger event to activate it
    private void OnTriggerEnter(Collider other)
        {
            StartCoroutine (Dial1());
        }

    IEnumerator Dial1()
        {
        //executes the specific block
        DialogFlow.ExecuteBlock(targetBlock);
        //this is required for a co-routine
        yield return new WaitForSeconds(0);
            //deactivates the trigger so it does not loop
            gameObject.SetActive(false);
        }

}

