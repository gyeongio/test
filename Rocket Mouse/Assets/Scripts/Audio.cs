using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public void Update()
    {
        float audio = GameObject.Find("Main Camera").GetComponent<AudioSource>().volume;
        bool m = GameObject.Find("Main Camera").GetComponent<AudioSource>().mute;
        if (m == true)
            PlayerPrefs.SetFloat("Audio", 0);
        else
        {
            PlayerPrefs.SetFloat("Audio", audio);
        }
        
    }
}
