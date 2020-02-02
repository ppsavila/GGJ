using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Audio manager padraozinho com um array de efeitos bonititinho, tem que so que pegar quem fez os audios para nao da merda;
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager getInstance()
    {
        return instance;
    }

    AudioSource audioSource;
    public AudioClip[] efeitos;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void playSong(int i)
    {
        audioSource.clip = efeitos[i];
        audioSource.Play();
    }

}
