using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {
    
    //SOUNDS
    [Header("Audio clips")]
    public List<AudioClip> asteroidSounds;

    public List<AudioClip> damageSounds;
    public AudioClip docking;
    public AudioClip undock; 
    
    
    public AudioClip resourceCollected;
    public AudioClip explosion;
    
    private AudioSource audiosrc;

    private static AudioManager _instance;

    public static AudioManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start() {
        audiosrc = GetComponent<AudioSource>();
    }

    public void Undock() {
        audiosrc.PlayOneShot(undock);
    }
    
    public void PlayDamage(Vector2 position, float volume) {
        AudioSource.PlayClipAtPoint(damageSounds[Random.Range(0, damageSounds.Count)], position, volume);
    }
    
    public void PlayExplosion(Vector2 position, float volume) {
        AudioSource.PlayClipAtPoint(explosion, Camera.main.transform.position, volume);
    }
    
    public void PlayAsteroidSound(Vector2 position, float volume) {
        AudioSource.PlayClipAtPoint(asteroidSounds[Random.Range(0, asteroidSounds.Count)], position, volume);
    }

    public void PlayDockingSound() {
        audiosrc.PlayOneShot(docking);
    }

    public void PlayAsteroidCollected(Vector2 position, float volume) {
        AudioSource.PlayClipAtPoint(resourceCollected, position, volume);
    }
}
