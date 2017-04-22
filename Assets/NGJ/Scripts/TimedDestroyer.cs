using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    float TimeToKill = 3;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(TimedDestruction());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetTime(float Time)
    {
        TimeToKill = Time;
    }
    IEnumerator TimedDestruction()
    {
        yield return new WaitForSeconds(TimeToKill);
        Destroy(this.gameObject);
    }
}
