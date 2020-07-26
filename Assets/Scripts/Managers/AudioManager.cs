using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSinglton<AudioManager>
{
    [SerializeField]
    private AudioClip _laserShotClip;
    private AudioSource _laserSource;
    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _explosionSource;
    [SerializeField]
    private AudioClip _PowerUpClip;
    private AudioSource _PowerUpSource;

    private void Start()
    {
        _laserSource = GameObject.Find("LaserShotAudio").GetComponent<AudioSource>();
        _explosionSource = GameObject.Find("Explosion").GetComponent<AudioSource>();
        _PowerUpSource = GameObject.Find("PowerUPSOUND").GetComponent<AudioSource>();
    }

    public void LaserShotAudioPlay()
    {
        _laserSource.clip = _laserShotClip;
        _laserSource.Play();
    }

    public void ExplosionPlay()
    {
        _explosionSource.clip = _explosionClip;
        _explosionSource.Play();
    }

    public void PowerUpPlay()
    {
        _PowerUpSource.clip = _PowerUpClip;
        _PowerUpSource.Play();
    }
}
