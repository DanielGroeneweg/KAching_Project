using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaitThenFireEvent : MonoBehaviour
{
    public UnityEvent TimePassed;
    public void StartWaiting(float timeToWait)
    {
        StartCoroutine(Wait(timeToWait));
    }
    private IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        TimePassed?.Invoke();
        StopCoroutine(Wait(timeToWait));
    }
}
