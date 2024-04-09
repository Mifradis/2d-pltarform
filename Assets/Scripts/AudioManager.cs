using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header (" Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header(" Audio Clip")]
    public AudioClip[] background;
    public AudioClip hit;
    public AudioClip death;
    public AudioClip jump;
    public AudioClip run;
    public AudioClip takeHit;
    public AudioClip attack;
    public AudioClip explosion;
    public AudioClip waterAttack;


    private void Start()
    {
        musicSource.clip = background[Random.Range(0,3)];
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
