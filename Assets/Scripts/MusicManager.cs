using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip musicClip;

    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = musicClip;
        audio.loop = true;
        audio.playOnAwake = false;
        audio.Play();
    }
}
