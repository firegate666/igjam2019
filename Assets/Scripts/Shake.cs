using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float YShake = 1.1f;
    public float XShake = 0;
    public float Time = 0.5f;
    public float Delay = 0.5f;
    
    // Update is called once per frame
    public void DoShake()
    {
        iTween.ShakePosition(gameObject, iTween.Hash("y", YShake, "x", XShake, "time", Time, "delay", Delay));
    }
}
