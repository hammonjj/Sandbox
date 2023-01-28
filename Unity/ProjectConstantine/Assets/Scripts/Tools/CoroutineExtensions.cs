using UnityEngine;
using System.Collections;

public class CoroutineExtensions
{
    //Wait X time before moving on
    protected IEnumerator CoroutineWaiter(float totalWaitTime)
    {
        var waitTimeDuration = 0f;
        while(waitTimeDuration < totalWaitTime)
        {
            waitTimeDuration += Time.deltaTime;
            yield return null;
        }
    }
}

