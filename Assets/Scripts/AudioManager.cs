using System.Collections;
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
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on Audio_Manager is Null!");
        }
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
}
