using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HelperMethods
{
    /// <summary>
    /// Executes the given Action after the given delay.
    /// </summary>
    /// <param name="behaviour">The MonoBehaviour that executes the action. The coroutine will be bound to this object.</param>
    /// <param name="action">The action to be executed.</param>
    /// <param name="delaySeconds">The desired delay</param>
    /// <returns>The Coroutine handling the delayed execution.</returns>
    public static Coroutine ExecuteDelayed(MonoBehaviour behaviour, Action action, float delaySeconds){
        return behaviour.StartCoroutine(ExecDelayedCoroutine(action, delaySeconds));
    }


    private static IEnumerator ExecDelayedCoroutine(Action action, float delaySeconds){
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
