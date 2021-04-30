using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip pointSound;
    public AudioClip incorrectPointSound;
    public void JumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void ActionSound()
    {
        audioSource.PlayOneShot(pointSound);
    }

    public void IncorrectActionSound()
    {
        audioSource.PlayOneShot(incorrectPointSound);
    }
}
