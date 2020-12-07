// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)


using System.Collections;
using UnityEngine;

namespace Fungus
{

    [CommandInfo("Flow",
        "WaitUntil",
        "Waits Until a variable returns true")]
    [AddComponentMenu("")]

    public class VariableWaitUntil : VariableCondition
    {

        // If a regular WaitUntil is too heavy, just make this float bigger and it works as a while->condition->waitforseconds instead
        private FloatData waitForSeconds_ = new FloatData(0f);

        public override void OnEnter()
        {

            bool condition = EvaluateCondition();
            //Debug.Log("Condition is: " + condition.ToString());
            StartCoroutine(Waiter());

        }

        IEnumerator Waiter()
        {

            // Evaluate every x seconds if a value is given, otherwise just do a classic WaitUntil
            if (waitForSeconds_ > 0f)
            {
                while (!EvaluateCondition())
                {
                    //Debug.Log("Condition is false");
                    yield return new WaitForSeconds(waitForSeconds_);
                };
            }
            else
            {
                yield return new WaitUntil(() => EvaluateCondition());
            };
            //Debug.Log("Condition is true");
            Continue();
        }

        public override bool OpenBlock()
        {
            return false;
        }


    }
}