using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource MenuClick;
    public AudioSource Fire;
    public AudioSource Water;
    public AudioSource Wood;
    public AudioSource Impact;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayMenuClick()
    {
        MenuClick.Play();
    }
    
    public void PlayFire()
    {
        Fire.Play();
    }
    
    public void PlayWater()
    {
        Water.Play();
    }
    
    public void PlayImpact()
    {
        Impact.Play();
    }
    
    public void PlayWood()
    {
        Wood.Play();
    }
}
