using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenMusic : MonoBehaviour
{
    new private AudioSource audio;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audio = GetComponent<AudioSource>();
    }
}
