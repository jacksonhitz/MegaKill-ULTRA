using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sfx;

    public AudioClip title;
    public AudioClip witch;
    public AudioClip hott;
    public AudioClip threes;

    public AudioClip gunShot;
    public AudioClip reload;
    public AudioClip empty;


    void Start()
    {
        music.clip = title;
        music.Play();
    }

    public void Gunshot()
    {
        sfx.clip = gunShot;
        sfx.Play();
    }
    public void Reload()
    {
        sfx.clip = reload;
        sfx.Play();
    }
    public void Empty()
    {
        sfx.clip = empty;
        sfx.Play();
        sfx.Play();

    }


}