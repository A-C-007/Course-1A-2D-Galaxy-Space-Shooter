﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioClip _powerupSoundClip;
    // Start is called before the first frame update
    [SerializeField]
    private AudioClip _warningSoundClip;
    [SerializeField]
    private AudioClip _shieldHitSoundClip;
    [SerializeField]
    private AudioClip _engineExplosionSoundClip;
    [SerializeField]
    private AudioClip _EnemyArrivingSoundClip;
    [SerializeField]
    private AudioSource _bgAudioSource;
    [SerializeField]
    private AudioClip _bgAudioClip;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on Audio_Manager is Null!");
        }

        _bgAudioClip = _bgAudioSource.GetComponent<AudioClip>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayExplosionSound()
    {
        _audioSource.PlayOneShot(_explosionSoundClip);
    }

    public void PlayPowerupSound()
    {
        _audioSource.PlayOneShot(_powerupSoundClip);
    }
    public void PlayWarningSound()
    {
        _audioSource.PlayOneShot(_warningSoundClip, 1);
    }
    public void PlayShieldHitSound()
    {
        _audioSource.PlayOneShot(_shieldHitSoundClip);
    }
    public void PlayEngineExplosionSound()
    {
        _audioSource.PlayOneShot(_engineExplosionSoundClip);
    }
    public void PlayEnemyArrivingSoundSound()
    {

        _bgAudioSource.clip = _EnemyArrivingSoundClip;
        _bgAudioSource.Play();
    }
}
