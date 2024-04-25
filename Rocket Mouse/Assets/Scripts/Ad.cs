using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ad : MonoBehaviour
{
    AudioSource audiosource;
    float a = 0;
    void Update()
    {
        audiosource = GetComponent<AudioSource>();
        a = PlayerPrefs.GetFloat("Audio");
        audiosource.volume = a;

    }
}
