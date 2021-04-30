using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;
    public void ClickSound()
    {
        audioSource.PlayOneShot(sound);
    }
}
